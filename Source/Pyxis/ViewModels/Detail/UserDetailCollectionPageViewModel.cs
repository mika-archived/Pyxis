using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailCollectionPageViewModel : ThumbnailableViewModel
    {
        private readonly IImageStoreService _imageStoreService;

        public INavigationService NavigationService { get; }

        public UserDetailCollectionPageViewModel(IImageStoreService imageStoreService,
                                                 INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            ThumbnailPath = PyxisConstants.DummyIcon;
        }

        private void Initialize(UserDetailParameter parameter)
        {
            SelectedIndex = (int) parameter.ProfileType;
            Username = parameter.Detail.User.Name;
            ScreenName = $"@{parameter.Detail.User.AccountName}";
            Url = parameter.Detail.Profile.Webpage;
            NavigateUrl = new Uri(parameter.Detail.Profile.Webpage);
            Thumbnailable = new PixivUserImage(parameter.Detail.User, _imageStoreService);
            Thumbnailable.ObserveProperty(v => v.ThumbnailPath)
                         .Where(v => !string.IsNullOrWhiteSpace(v))
                         .ObserveOnUIDispatcher()
                         .Subscribe(v => ThumbnailPath = v)
                         .AddTo(this);
            Parameter = new List<string>
            {
                (string) new DetailByIdParameter {Id = parameter.Detail.User.Id}.ToJson(),
                (string) new UserDetailParameter {Detail = parameter.Detail, ProfileType = ProfileType.Work}.ToJson(),
                (string)
                    new UserDetailParameter {Detail = parameter.Detail, ProfileType = ProfileType.Favorite}.ToJson()
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

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
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
    }
}