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
        private PixivUser _pixivUser;

        public IllustDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                         INavigationService navigationService)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            IconPath = PyxisConstants.DummyIcon;
        }

        private void Initialize(IllustDetailParameter parameter)
        {
            _illust = parameter.Illust;
            Title = _illust.Title;
            Description = _illust.Caption.Replace("<br />", Environment.NewLine);
            Username = _illust.User.Name;
            Height = _illust.Height;
            Width = _illust.Width;
            Thumbnailable = new PixivImage(_illust, _imageStoreService, true);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
            _pixivUser = new PixivUser(_illust.User, _imageStoreService);
            _pixivUser.ObserveProperty(w => w.ThumbnailPath)
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => IconPath = w).AddTo(this);
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

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
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

        #region IconPath

        private string _iconPath;

        public string IconPath
        {
            get
            {
                if (_iconPath == PyxisConstants.DummyIcon)
                    _pixivUser.ShowThumbnail();
                return _iconPath;
            }
            set { SetProperty(ref _iconPath, value); }
        }

        #endregion

        #region Height

        private int _height;

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        #endregion

        #region Width

        private int _width;

        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        #endregion
    }
}