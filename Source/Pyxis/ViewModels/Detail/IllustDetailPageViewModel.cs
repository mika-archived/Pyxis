using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class IllustDetailPageViewModel : ThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private IIllust _illust;

        public IllustDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                         INavigationService navigationService)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
        }

        private void Initialize(IllustDetailParameter parameter)
        {
            _illust = parameter.Illust;
            Title = _illust.Title;
            Thumbnailable = new PixivImage(_illust, _imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = e.Parameter as IllustDetailParameter;
            //ParameterBase.ToObject<IllustDetailParameter>(e.Parameter?.ToString(), true);
            Initialize(parameter);
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

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