using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Extensions;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailCollectionPageViewModel : ThumbnailableViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivFavorite _pixivFavorite;
        private PixivWork _pixivWork;
        public INavigationService NavigationService { get; }
        public IncrementalObservableCollection<ThumbnailableViewModel> Collection { get; }

        public UserDetailCollectionPageViewModel(IImageStoreService imageStoreService,
                                                 INavigationService navigationService, IPixivClient pixivClient)
        {
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Collection = new IncrementalObservableCollection<ThumbnailableViewModel>();
        }

        private void Initialize(UserDetailParameter parameter)
        {
            SelectedIndex = (int) parameter.ProfileType;
            if (parameter.ProfileType == ProfileType.Work)
                SubSelectedIndex1 = (int) parameter.ContentType;
            else
                SubSelectedIndex2 = parameter.ContentType == ContentType.Illust ? 0 : 1;
            Username = parameter.Detail.User.Name;
            ScreenName = $"@{parameter.Detail.User.AccountName}";
            Url = parameter.Detail.Profile.Webpage;
            if (!string.IsNullOrWhiteSpace(parameter.Detail.Profile.Webpage))
                NavigateUrl = new Uri(parameter.Detail.Profile.Webpage);
            Thumbnailable = new PixivUserImage(parameter.Detail.User, _imageStoreService);
            var param1 = new UserDetailParameter
            {
                Detail = parameter.Detail,
                ProfileType = ProfileType.Work,
                ContentType = ContentType.Illust
            };
            var param2 = (UserDetailParameter) param1.Clone();
            param2.ProfileType = ProfileType.Favorite;
            Parameter = new List<string>
            {
                (string) new DetailByIdParameter {Id = parameter.Detail.User.Id}.ToJson(),
                (string) param1.ToJson(),
                (string) param2.ToJson()
            };

            if (parameter.ProfileType == ProfileType.Work)
            {
                InitializeSubMenu(param1, true);
                _pixivWork = new PixivWork(parameter.Detail.User.Id, parameter.ContentType, _pixivClient);
                if (parameter.ContentType != ContentType.Novel)
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Illusts, CreatePixivImage);
                else
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Novels, CreatePixivNovel);
            }
            else
            {
                InitializeSubMenu(param1, false);
                _pixivFavorite = new PixivFavorite(_pixivClient);
                if (parameter.ContentType != ContentType.Novel)
                    ModelHelper.ConnectTo(Collection, _pixivFavorite, w => w.ResultIllusts, CreatePixivImage);
                else
                    ModelHelper.ConnectTo(Collection, _pixivFavorite, w => w.ResultNovels, CreatePixivNovel);
                _pixivFavorite.Query(new FavoriteOptionParameter
                {
                    Restrict = RestrictType.Public,
                    Type = parameter.ContentType.ToSearchType(),
                    Tag = "",
                    UserId = parameter.Detail.User.Id
                });
            }
        }

        private void InitializeSubMenu(UserDetailParameter param, bool mode)
        {
            IsEnabledSubMenu = mode;
            SubParameters = ParamGen.GenerateRaw(param, w => w.ContentType)
                                    .Do(w => w.ProfileType = (ProfileType) SelectedIndex)
                                    .Select(w => (string) w.ToJson())
                                    .ToList();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<UserDetailParameter>((string) e?.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        #endregion

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion

        #region SubSelectedIndex1

        private int _subSelectedIndex1;

        public int SubSelectedIndex1
        {
            get { return _subSelectedIndex1; }
            set { SetProperty(ref _subSelectedIndex1, value); }
        }

        #endregion

        #region SubSelectedIndex2

        private int _subSelectedIndex2;

        public int SubSelectedIndex2
        {
            get { return _subSelectedIndex2; }
            set { SetProperty(ref _subSelectedIndex2, value); }
        }

        #endregion

        #region Username

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        #endregion

        #region ScreenName

        private string _screenName;

        public string ScreenName
        {
            get { return _screenName; }
            set { SetProperty(ref _screenName, value); }
        }

        #endregion

        #region Url

        private string _url;

        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        #endregion

        #region NavigateUrl

        private Uri _navigateUrl;

        public Uri NavigateUrl
        {
            get { return _navigateUrl; }
            set { SetProperty(ref _navigateUrl, value); }
        }

        #endregion

        #region Parameter

        private List<string> _parameter;

        public List<string> Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        #endregion

        #region IsEnabledSubMenu

        private bool _isEnabledSubMenu;

        public bool IsEnabledSubMenu
        {
            get { return _isEnabledSubMenu; }
            set { SetProperty(ref _isEnabledSubMenu, value); }
        }

        #endregion

        #region SubParameters

        private List<string> _subParameters;

        public List<string> SubParameters
        {
            get { return _subParameters; }
            set { SetProperty(ref _subParameters, value); }
        }

        #endregion
    }
}