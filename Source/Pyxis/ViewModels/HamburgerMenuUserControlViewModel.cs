using System;

using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class HamburgerMenuUserControlViewModel : ViewModel
    {
        private readonly IAccountService _accountService;

        public HamburgerMenuUserControlViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            _accountService.OnLoggedIn += OnUserAction;
            _accountService.OnLoggedOut += OnUserAction;
            UpdateUserInformation();
        }

        private void OnUserAction(object sender, EventArgs eventArgs) => UpdateUserInformation();

        private void UpdateUserInformation()
        {
            if (_accountService.Account != null)
            {
                Username = _accountService.Account.Name;
                Thumbnail = _accountService.Account.ProfileImageUrls.Medium;
            }
            else
            {
                Username = "ゲスト";
                Thumbnail = PyxisConstants.DefaultIcon;
            }
        }

        #region Username

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        #endregion

        #region Thumbnail

        private string _thumbnail;

        public string Thumbnail
        {
            get { return _thumbnail; }
            set { SetProperty(ref _thumbnail, value); }
        }

        #endregion
    }
}