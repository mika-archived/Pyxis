using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Prism.Windows.Navigation;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    public class LoginPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ReactiveProperty<string> Username { get; }
        public ReactiveProperty<string> Password { get; }
        public ReactiveCommand LoginCommand { get; }

        public LoginPageViewModel(IAccountService accountService, IDialogService dialogService, INavigationService navigationService)
        {
            _accountService = accountService;
            _dialogService = dialogService;
            _navigationService = navigationService;

            Username = new ReactiveProperty<string>();
            Password = new ReactiveProperty<string>();
            LoginCommand = new[]
            {
                Username.Select(string.IsNullOrWhiteSpace),
                Password.Select(string.IsNullOrWhiteSpace)
            }.CombineLatestValuesAreAllFalse()
             .ToReactiveCommand();
            LoginCommand.Subscribe(async w => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            await _accountService.LoginAsync(Username.Value, Password.Value);
            if (_accountService.Account == null)
            {
                await _dialogService.ShowErrorDialogAsync("エラー", "メールアドレスもしくはパスワードが間違えているため、ログインに失敗しました。");
                return;
            }
            _navigationService.Navigate("Home", null);
        }
    }
}