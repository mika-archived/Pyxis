using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.New
{
    public class FollowMypixivNewPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        public INavigationService NavigationService { get; }

        public FollowMypixivNewPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            NavigationService = navigationService;
            IsLoggedInRequired = !_accountService.IsLoggedIn;
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public void OnLoginButtonTapped() => NavigationService.Navigate("Account.Login", "New.FollowMypixivNew");

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            IsLoggedInRequired = !_accountService.IsLoggedIn;
        }

        #endregion

        #region IsEnabled

        private bool _isLoggedInRequired;

        public bool IsLoggedInRequired
        {
            get { return _isLoggedInRequired; }
            set { SetProperty(ref _isLoggedInRequired, value); }
        }

        #endregion
    }
}