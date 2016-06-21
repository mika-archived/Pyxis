﻿using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Following
{
    public class PrivateFollowingPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        public INavigationService NavigationService { get; }

        public PrivateFollowingPageViewModel(IAccountService accountService, INavigationService navigationService)
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
        }

        #endregion

        private void RedirectToLoginPageWhenNoLogin()
        {
            NavigationService.Navigate("Error.LoginRequired", "Following.PrivateFollowing");
        }
    }
}