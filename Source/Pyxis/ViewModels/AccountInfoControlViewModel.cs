using System;
using System.Reactive.Linq;

using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    public class AccountInfoControlViewModel : ThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IDisposable _disposable;
        private readonly IImageStoreService _imageStoreService;

        public AccountInfoControlViewModel(IAccountService accountService, IImageStoreService imageStoreService)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            Username = "ログインしていません";
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                                    .ObserveOnUIDispatcher()
                                    .Subscribe(w =>
                                    {
                                        if (_accountService.IsLoggedIn)
                                            LoggedIn();
                                        else
                                            LoggedOut();
                                    }).AddTo(this);
        }

        private void LoggedIn()
        {
            Username = _accountService.LoggedInAccount.Name;
            Thumbnailable = new PixivUserImage(_accountService.LoggedInAccount, _imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        private void LoggedOut()
        {
            Username = "ログインしていません";
            ThumbnailPath = PyxisConstants.DummyIcon;
        }

        #region Username

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        #endregion
    }
}