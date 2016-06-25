using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class MangaDetailPageViewModel : TappableThumbnailViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private IIllust _illust;
        private PixivUser _pixivUser;
        public ObservableCollection<PixivTagViewModel> Tags { get; }

        public MangaDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                        INavigationService navigationService)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            Tags = new ObservableCollection<PixivTagViewModel>();
            ThumbnailPath = PyxisConstants.DummyImage;
            IconPath = PyxisConstants.DummyIcon;
        }

        private void Initialize(IllustDetailParameter parameter)
        {
            _illust = parameter.Illust;
            Title = _illust.Title;
            Description = _illust.Caption.Replace("<br />", Environment.NewLine);
            CreatedAt = _illust.CreateDate.ToString("g");
            Username = _illust.User.Name;
            View = _illust.TotalView;
            Bookmark = _illust.TotalBookmarks;
            Height = _illust.Height;
            Width = _illust.Width;
            _illust.Tags.ForEach(w => Tags.Add(new PixivTagViewModel(w, _navigationService)));
            Thumbnailable = new PixivImage(_illust, _imageStoreService, true);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
            _pixivUser = new PixivUser(_illust.User, _imageStoreService);
            _pixivUser.ObserveProperty(w => w.ThumbnailPath)
                      .Where(w => !string.IsNullOrWhiteSpace(w))
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => IconPath = w).AddTo(this);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new IllustDetailParameter {Illust = _illust};
            _navigationService.Navigate("Detail.MangaView", parameter.ToJson());
        }

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<IllustDetailParameter>((string) e.Parameter);
            if (parameter == null && viewModelState.ContainsKey("Illust"))
                parameter = viewModelState["Illust"] as IllustDetailParameter;
            Initialize(parameter);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
                                              bool suspending)
        {
            if (suspending)
                viewModelState["Illust"] = new IllustDetailParameter {Illust = _illust}.ToJson();
            base.OnNavigatingFrom(e, viewModelState, suspending);
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

        #region CreatedAt

        private string _createdAt;

        public string CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
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

        #region View

        private int _view;

        public int View
        {
            get { return _view; }
            set { SetProperty(ref _view, value); }
        }

        #endregion

        #region Bookmark

        private int _bookmark;

        public int Bookmark
        {
            get { return _bookmark; }
            set { SetProperty(ref _bookmark, value); }
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