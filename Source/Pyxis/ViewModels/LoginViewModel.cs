using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Prism.Windows.Navigation;

using Pyxis.Constants;
using Pyxis.Enums;
using Pyxis.Models;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly PixivClient _pixivClient;
        private List<Illust> _illustCollection;
        public ReactiveProperty<List<string>> ImageCollection { get; set; }
        public ReactiveProperty<string> Username { get; }
        public ReactiveProperty<string> Password { get; }
        public AsyncReactiveCommand LoginCommand { get; }

        public LoginViewModel(PixivClient pixivClient, IAccountService accountService, IDialogService dialogService, INavigationService navigationService)
        {
            _pixivClient = pixivClient;
            _accountService = accountService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            ImageCollection = new ReactiveProperty<List<string>>();
            Username = new ReactiveProperty<string>();
            Password = new ReactiveProperty<string>();
            LoginCommand = new[]
            {
                Username.Select(string.IsNullOrWhiteSpace),
                Password.Select(string.IsNullOrWhiteSpace)
            }.CombineLatestValuesAreAllFalse().ToAsyncReactiveCommand();
            LoginCommand.Subscribe(async w => await LoginAsync()).AddTo(CompositeDisposable);

            Task.Run(LoadBackgrounds);
        }

        private async Task LoginAsync()
        {
            await _accountService.LoginAsync(Username.Value, Password.Value);
            if (_accountService.CurrentUser == null)
            {
                await _dialogService.ShowErrorDialogAsync("認証エラー", "メールアドレスもしくはパスワードが間違えているため、ログインに失敗しました。");
                return;
            }
            _navigationService.Navigate(PageTokens.MainPage, new TransitionParameter {Mode = TransitionMode.LoginRedirect}.ToQueryString());
        }

        private async Task LoadBackgrounds()
        {
            _illustCollection = (await _pixivClient.Walkthrough.IllustsAsync()).Illusts.ToList();
            ImageCollection.Value = _illustCollection.Select(w => w.ImageUrls.Medium).ToList();
        }
    }
}