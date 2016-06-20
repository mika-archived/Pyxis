using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

using Pyxis.Services.Interfaces;

namespace Pyxis.ViewModels.New
{
    public class MypixivNewPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        public INavigationService NavigationService { get; }

        public MypixivNewPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            NavigationService = navigationService;
            IsLoggedInRequired = !_accountService.IsLoggedIn;
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public void OnLoginButtonTapped() => NavigationService.Navigate("Account.Login", "New.MypixivNew");

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