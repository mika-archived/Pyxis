using System.Reactive.Linq;
using System.Threading.Tasks;

using Pyxis.Models.Enum;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    public class LoginPageViewModel : ViewModel
    {
        private readonly IDialogService _dialogService;

        public ReactiveProperty<string> Username { get; }
        public ReactiveProperty<string> Password { get; }
        public AsyncReactiveCommand LoginCommand { get; }

        public LoginPageViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            Username = new ReactiveProperty<string>();
            Password = new ReactiveProperty<string>();
            LoginCommand = new[]
            {
                Username.Select(string.IsNullOrWhiteSpace),
                Password.Select(string.IsNullOrWhiteSpace)
            }.CombineLatestValuesAreAllFalse()
             .ToAsyncReactiveCommand();
            LoginCommand.Subscribe(async w => await LoginAsync()).AddTo(this);
        }

        private async Task LoginAsync()
        {
            await AccountService.LoginAsync(Username.Value, Password.Value);
            if (AccountService.Account == null)
            {
                await _dialogService.ShowErrorDialogAsync("エラー", "メールアドレスもしくはパスワードが間違えているため、ログインに失敗しました。");
                return;
            }
            NavigationService.Navigate("Home", new TransitionParameter {Mode = TransitionMode.Redirect}.ToQuery());
        }
    }
}