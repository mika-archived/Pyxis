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
            ThumbnailPath = "http://placehold.jp/24/cccccc/999999/48x48.png?text=A";

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(w =>
            {
                if (_accountService.IsLoggedIn)
                    LoggedIn();
            }).AddTo(this);
        }

        private void LoggedIn()
        {
            _disposable.Dispose();
            Thumbnailable = new PixivUser(_accountService.LoggedInAccount, _imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }
    }
}