using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;

namespace Pyxis.ViewModels.Follower
{
    public class MainFollowerPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public MainFollowerPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            RunHelper.RunLater(RedirectToLoginPageWhenNoLogin, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        private void RedirectToLoginPageWhenNoLogin()
        {
            _navigationService.Navigate("Error.LoginRequired", "Follower.MainFollower");
        }
    }
}