using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Account
{
    public class LoginPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private RedirectParameter _parameter;

        public ReactiveProperty<string> Username { get; }
        public ReactiveProperty<string> Password { get; }
        public ReactiveCommand LoginCommand { get; }

        public LoginPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            Username = new ReactiveProperty<string>(string.Empty);
            Password = new ReactiveProperty<string>(string.Empty);
            LoginCommand = new[]
            {
                Username.Select(string.IsNullOrWhiteSpace),
                Password.Select(string.IsNullOrWhiteSpace)
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand();
            LoginCommand.Subscribe(async w => await LoginAsync()).AddTo(this);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _parameter = ParameterBase.ToObject<RedirectParameter>(e?.Parameter.ToString());
            _navigationService.RemoveAllPages("Error.LoginRequired");
        }

        #endregion

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task LoginAsync()
        {
            IsProcessing = true;
            _accountService.Save(new AccountInfo(Username.Value, Password.Value, Guid.NewGuid().ToString().Replace("-", "")));
            await _accountService.Login();
            IsProcessing = false;

            if (!_accountService.IsLoggedIn)
            {
                IsLoginFailure = true;
                return;
            }

            _navigationService.Navigate(_parameter.RedirectTo, _parameter.Parameter?.ToJson());
        }

        #region IsProcessing

        private bool _isProcessing;

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { SetProperty(ref _isProcessing, value); }
        }

        #endregion

        #region IsLoginFailure

        private bool _isLoginFailure;

        public bool IsLoginFailure
        {
            get { return _isLoginFailure; }
            set { SetProperty(ref _isLoginFailure, value); }
        }

        #endregion
    }
}