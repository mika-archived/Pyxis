using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings.Extensions;

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
            SubSelectedIndex = (int) parameter.ContentType;
            Username = parameter.Detail.User.Name;
            ScreenName = $"@{parameter.Detail.User.AccountName}";
            Url = parameter.Detail.Profile.Webpage;
            if (!string.IsNullOrWhiteSpace(parameter.Detail.Profile.Webpage))
                NavigateUrl = new Uri(parameter.Detail.Profile.Webpage);
            Thumbnailable = new PixivUserImage(parameter.Detail.User, _imageStoreService);
            Thumbnailable.ObserveProperty(v => v.ThumbnailPath)
                         .Where(v => !string.IsNullOrWhiteSpace(v))
                         .ObserveOnUIDispatcher()
                         .Subscribe(v => ThumbnailPath = v)
                         .AddTo(this);
            var param1 = new UserDetailParameter
            {
                Detail = parameter.Detail,
                ProfileType = ProfileType.Work,
                ContentType = ContentType.Illust
            };
            var param2 = param1.Clone();
            param2.ProfileType = ProfileType.Favorite;
            Parameter = new List<string>
            {
                (string) new DetailByIdParameter {Id = parameter.Detail.User.Id}.ToJson(),
                (string) param1.ToJson(),
                (string) param2.ToJson()
            };

            if (parameter.ProfileType == ProfileType.Work)
            {
                InitializeSubMenu(param1);
                _pixivWork = new PixivWork(parameter.Detail.User.Id, parameter.ContentType, _pixivClient);
                if (parameter.ContentType != ContentType.Novel)
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Illusts, CreatePixivImage);
                else
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Novels, CreatePixivNovel);
            }
        }

        private void InitializeSubMenu(UserDetailParameter param)
        {
            IsEnabledSubMenu = true;
            var param3 = param.Clone();
            param3.ContentType = ContentType.Manga;
            var param4 = param.Clone();
            param4.ContentType = ContentType.Novel;
            SubParameters = new List<string>
            {
                (string) param.ToJson(),
                (string) param3.ToJson(),
                (string) param4.ToJson()
            };
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

        #region SubSelectedIndex

        private int _subSelectedIndex;

        public int SubSelectedIndex
        {
            get { return _subSelectedIndex; }
            set { SetProperty(ref _subSelectedIndex, value); }
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