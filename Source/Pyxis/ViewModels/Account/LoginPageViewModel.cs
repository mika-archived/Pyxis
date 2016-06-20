using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Account
{
    public class LoginPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private string _nextPageToken;

        public ReactiveProperty<string> Username { get; }
        public ReactiveProperty<string> Password { get; }
        public ReactiveCommand LoginCommand { get; }

        public LoginPageViewModel(IAccountService accountService, INavigationService navigationService,
                                  IPixivClient pixivClient)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;

            Username = new ReactiveProperty<string>(string.Empty);
            Password = new ReactiveProperty<string>(string.Empty);
            LoginCommand = new[]
            {
                Username.Select(string.IsNullOrWhiteSpace),
                Password.Select(string.IsNullOrWhiteSpace)
            }.CombineLatestValuesAreAllFalse().ToReactiveCommand();
            LoginCommand.Subscribe(async w => await LoginAsync()).AddTo(this);
        }

        private async Task LoginAsync()
        {
            IsProcessing = true;
            // ReSharper disable InconsistentNaming
            var account = await _pixivClient.Authorization
                                            .Login(get_secure_url => 1,
                                                   grant_type => "password",
                                                   client_secret => "HP3RmkgAmEGro0gn1x9ioawQE8WMfvLXDz3ZqxpK",
                                                   device_token => Guid.NewGuid().ToString().Replace("-", ""),
                                                   password => Password.Value,
                                                   client_id => "bYGKuGVw91e0NMfPGp44euvGt59s",
                                                   username => Username.Value);
            if (account == null)
                return;
            _accountService.Save(new AccountInfo(Username.Value, Password.Value, account.User));
            IsProcessing = false;

            _navigationService.Navigate(_nextPageToken, null);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _nextPageToken = e.Parameter as string;
        }

        #endregion

        #region IsProcessing

        private bool _isProcessing;

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { SetProperty(ref _isProcessing, value); }
        }

        #endregion
    }
}