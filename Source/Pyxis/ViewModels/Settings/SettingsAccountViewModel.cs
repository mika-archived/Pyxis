using System.Windows.Input;

using Prism.Commands;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Settings
{
    public class SettingsAccountViewModel : ViewModel
    {
        private readonly IAccountService _accountService;

        public string Username => $"@{_accountService.LoggedInAccount.AccountName}";

        public SettingsAccountViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            IsLoggedIn = accountService.IsLoggedIn;
        }

        #region LogoutCommand

        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

        private void Logout()
        {
            _accountService.Clear();
            IsLoggedIn = false;
        }

        #endregion

        #region IsLoggedIn

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        #endregion
    }
}