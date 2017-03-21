using System;
using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class HamburgerMenuUserControlViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICacheService _cacheService;

        public HamburgerMenuUserControlViewModel(IAccountService accountService, ICacheService cacheService)
        {
            _accountService = accountService;
            _cacheService = cacheService;
            _accountService.OnLoggedIn += OnUserAction;
            _accountService.OnLoggedOut += OnUserAction;
            Thumbnail = PyxisConstants.DefaultIcon;
            RunHelper.RunLaterUIAsync(UpdateUserInformation, TimeSpan.FromMilliseconds(1));
        }

        private async void OnUserAction(object sender, EventArgs eventArgs) => await UpdateUserInformation();

        private async Task UpdateUserInformation()
        {
            if (_accountService.Account != null)
            {
                Username = _accountService.Account.Name;
                Thumbnail = await _cacheService.SaveFileAsync(_accountService.Account.ProfileImageUrls.Medium);
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