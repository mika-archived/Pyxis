using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Extensions;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailCollectionPageViewModel : MultipleThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private string _id;
        private bool _isOffline;
        private PixivFavorite _pixivFavorite;
        private PixivWork _pixivWork;
        public INavigationService NavigationService { get; }
        public IncrementalObservableCollection<ThumbnailableViewModel> Collection { get; }

        public UserDetailCollectionPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                                 IImageStoreService imageStoreService,
                                                 INavigationService navigationService, IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Collection = new IncrementalObservableCollection<ThumbnailableViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<UserDetailParameter>((string) e?.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Events

        #region FollowCommand

        private ICommand _followCommand;

        public ICommand FollowCommand => _followCommand ?? (_followCommand = new DelegateCommand(Follow, CanFollow));

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async void Follow()
        {
            if (IsFollowing)
                await _pixivClient.User.Follow.DeleteAsunc(user_id => _id, restrict => "public");
            else
                await _pixivClient.User.Follow.AddAsync(user_id => _id, restrict => "public");
            IsFollowing = !IsFollowing;
            var param = ParameterBase.ToObject<UserDetailParameter>(Parameter[1]);
            ((User) param.Detail.User).IsFollowed = IsFollowing;
            Initialize(param, false);
        }

        private bool CanFollow() => _accountService.IsLoggedIn && !_isOffline;

        #endregion

        #endregion

        #region Initializers

        private void Initialize(UserDetailParameter parameter, bool full = true)
        {
            _categoryService.UpdateCategory();
            if (parameter == null)
            {
                // オフライン
                IsFollowing = false;
                _isOffline = true;
                return;
            }
            SelectedIndex = (int) parameter.ProfileType;
            if (parameter.ProfileType == ProfileType.Work)
                SubSelectedIndex1 = (int) parameter.ContentType;
            else
                SubSelectedIndex2 = parameter.ContentType == ContentType.Illust ? 0 : 1;
            Username = parameter.Detail.User.Name;
            ScreenName = $"@{parameter.Detail.User.AccountName}";
            Url = parameter.Detail.Profile.Webpage;
            IsFollowing = parameter.Detail.User.IsFollowed;
            _id = parameter.Detail.User.Id;
            if (!string.IsNullOrWhiteSpace(parameter.Detail.Profile.Webpage))
                NavigateUrl = new Uri(parameter.Detail.Profile.Webpage);
            if (full)
            {
                Thumbnailable = new PixivUserImage(parameter.Detail.User, _imageStoreService);
                if (string.IsNullOrWhiteSpace(parameter.Detail.Profile.BackgroundImageUrl))
                    Thumbnailable2 = new PixivUserImage(parameter.Detail.User, _imageStoreService);
                else
                    Thumbnailable2 = new PixivUrlImage(parameter.Detail.Profile.BackgroundImageUrl, _imageStoreService);
            }
            var param1 = new UserDetailParameter
            {
                Detail = parameter.Detail,
                ProfileType = ProfileType.Work,
                ContentType = ContentType.Illust
            };
            var param2 = (UserDetailParameter) param1.Clone();
            param2.ProfileType = ProfileType.Favorite;
            Parameter = new List<object>
            {
                new DetailByIdParameter {Id = parameter.Detail.User.Id},
                param1,
                param2
            };

            if (parameter.ProfileType == ProfileType.Work)
            {
                InitializeSubMenu(param1, true);
                if (!full)
                    return;
                _pixivWork = new PixivWork(parameter.Detail.User.Id, parameter.ContentType, _pixivClient);
                if (parameter.ContentType != ContentType.Novel)
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Illusts, CreatePixivImage).AddTo(this);
                else
                    ModelHelper.ConnectTo(Collection, _pixivWork, w => w.Novels, CreatePixivNovel).AddTo(this);
            }
            else
            {
                InitializeSubMenu(param1, false);
                if (!full)
                    return;
                _pixivFavorite = new PixivFavorite(_pixivClient);
                if (parameter.ContentType != ContentType.Novel)
                    ModelHelper.ConnectTo(Collection, _pixivFavorite, w => w.ResultIllusts, CreatePixivImage)
                               .AddTo(this);
                else
                    ModelHelper.ConnectTo(Collection, _pixivFavorite, w => w.ResultNovels, CreatePixivNovel).AddTo(this);
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
                                    .Cast<object>()
                                    .ToList();
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

        #region IsFollowing

        private bool _isFollowing;

        public bool IsFollowing
        {
            get { return _isFollowing; }
            set { SetProperty(ref _isFollowing, value); }
        }

        #endregion

        #region Parameter

        private List<object> _parameter;

        public List<object> Parameter
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

        private List<object> _subParameters;

        public List<object> SubParameters
        {
            get { return _subParameters; }
            set { SetProperty(ref _subParameters, value); }
        }

        #endregion
    }
}