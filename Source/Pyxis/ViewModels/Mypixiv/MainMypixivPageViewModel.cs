﻿using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.ViewModels.Mypixiv
{
    public class MainMypixivPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public MainMypixivPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
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
            _navigationService.Navigate("Error.LoginRequired", "Mypixiv.MainMypixiv");
        }
    }
}