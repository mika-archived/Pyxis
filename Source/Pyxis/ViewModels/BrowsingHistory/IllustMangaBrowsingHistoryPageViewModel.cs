using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.ViewModels.BrowsingHistory
{
    public class IllustMangaBrowsingHistoryPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        public INavigationService NavigationService { get; }

        public IllustMangaBrowsingHistoryPageViewModel(IAccountService accountService,
                                                       INavigationService navigationService)
        {
            _accountService = accountService;
            NavigationService = navigationService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (!_accountService.IsLoggedIn)
                RunHelper.RunLater(RedirectToLoginPageWhenNoLogin, TimeSpan.FromMilliseconds(10));
            else if (!_accountService.IsPremium)
                RunHelper.RunLater(RedirectToPremiumPageWhenNoPremium, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        private void RedirectToLoginPageWhenNoLogin()
        {
            NavigationService.Navigate("Error.LoginRequired", "BrowsingHistory.IllustMangaBrowsingHistory");
        }

        private void RedirectToPremiumPageWhenNoPremium()
        {
            NavigationService.Navigate("Error.PremiumRequired", null);
        }
    }
}