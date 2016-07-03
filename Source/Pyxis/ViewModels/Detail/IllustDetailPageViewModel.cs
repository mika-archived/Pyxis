using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class IllustDetailPageViewModel : ThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private int _count;
        private IIllust _illust;
        private bool _isRequested;
        private PixivComment _pixivComment;
        private PixivDetail _pixivDetail;
        private PixivRelated _pixivRelated;
        private PixivUserImage _pixivUser;

        public ObservableCollection<PixivTagViewModel> Tags { get; }
        public ObservableCollection<PixivCommentViewModel> Comments { get; }
        public IncrementalObservableCollection<PixivThumbnailViewModel> RelatedItems { get; }

        public IllustDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                         INavigationService navigationService, IPixivClient pixivClient)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            Tags = new ObservableCollection<PixivTagViewModel>();
            Comments = new ObservableCollection<PixivCommentViewModel>();
            RelatedItems = new IncrementalObservableCollection<PixivThumbnailViewModel>();
            ThumbnailPath = PyxisConstants.DummyImage;
            IconPath = PyxisConstants.DummyIcon;
        }

        #region Events

        public void OnTappedUserIcon()
        {
            var parameter = new UserDetailParameter {User = _illust.User};
            _navigationService.Navigate("Detail.UserDetail", parameter.ToJson());
        }

        #endregion

        #region Initializers

        private void Initialize(IllustDetailParameter parameter)
        {
            _count = 0;
            _illust = parameter.Illust;
            Title = _illust.Title;
            ConvertValues = new List<object> {_illust.Caption, _navigationService};
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
            _pixivUser = new PixivUserImage(_illust.User, _imageStoreService);
            _pixivUser.ObserveProperty(w => w.ThumbnailPath)
                      .Where(w => !string.IsNullOrWhiteSpace(w))
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => IconPath = w).AddTo(this);
            _pixivComment = new PixivComment(_illust, _pixivClient);
            _pixivComment.Fetch();
            _pixivComment.Comments.ObserveAddChanged()
                         .Where(w => ++_count <= 5)
                         .Select(CreatePixivComment)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => Comments.Add(w))
                         .AddTo(this);
            _pixivRelated = new PixivRelated(_illust, _pixivClient);
            ModelHelper.ConnectTo(RelatedItems, _pixivRelated, w => w.RelatedIllusts, CreatePixivImage);
        }

        private void Initialize(DetailByIdParameter parameter)
        {
            _count = 0;
            _pixivDetail = new PixivDetail(parameter.Id, SearchType.IllustsAndManga, _pixivClient);
            _pixivDetail.ObserveProperty(w => w.IllustDetail)
                        .Where(w => w != null)
                        .ObserveOnUIDispatcher()
                        .Subscribe(w =>
                        {
                            _illust = w;
                            Title = _illust.Title;
                            ConvertValues = new List<object> {_illust.Caption, _navigationService};
                            CreatedAt = _illust.CreateDate.ToString("g");
                            Username = _illust.User.Name;
                            View = _illust.TotalView;
                            Bookmark = _illust.TotalBookmarks;
                            Height = _illust.Height;
                            Width = _illust.Width;
                            _illust.Tags.ForEach(v => Tags.Add(new PixivTagViewModel(v, _navigationService)));
                            Thumbnailable = new PixivImage(_illust, _imageStoreService, true);
                            Thumbnailable.ObserveProperty(v => v.ThumbnailPath)
                                         .Where(v => !string.IsNullOrWhiteSpace(v))
                                         .ObserveOnUIDispatcher()
                                         .Subscribe(v => ThumbnailPath = v)
                                         .AddTo(this);
                            _pixivUser = new PixivUserImage(_illust.User, _imageStoreService);
                            _pixivUser.ObserveProperty(v => v.ThumbnailPath)
                                      .Where(v => !string.IsNullOrWhiteSpace(v))
                                      .ObserveOnUIDispatcher()
                                      .Subscribe(v => IconPath = v).AddTo(this);
                            RunHelper.RunLater(_pixivUser.ShowThumbnail, TimeSpan.FromMilliseconds(100));
                            _pixivComment = new PixivComment(_illust, _pixivClient);
                            _pixivComment.Fetch();
                            _pixivComment.Comments.ObserveAddChanged()
                                         .Where(v => ++_count <= 5)
                                         .Select(CreatePixivComment)
                                         .ObserveOnUIDispatcher()
                                         .Subscribe(v => Comments.Add(v))
                                         .AddTo(this);
                            _pixivRelated = new PixivRelated(_illust, _pixivClient);
                            ModelHelper.ConnectTo(RelatedItems, _pixivRelated, v => v.RelatedIllusts, CreatePixivImage);
                        }).AddTo(this);
            _pixivDetail.Fetch();
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, _navigationService);

        private PixivCommentViewModel CreatePixivComment(IComment w) =>
            new PixivCommentViewModel(w, _imageStoreService);

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<IllustDetailParameter>((string) e.Parameter);
            if (parameter.Illust == null && viewModelState.ContainsKey("Illust"))
                parameter = viewModelState["Illust"] as IllustDetailParameter;
            if (parameter.Illust != null)
                Initialize(parameter);
            else
            {
                var param = ParameterBase.ToObject<DetailByIdParameter>((string) e.Parameter);
                Initialize(param);
            }
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

        #region ConvertValues

        private List<object> _convertValues;

        public List<object> ConvertValues
        {
            get { return _convertValues; }
            set { SetProperty(ref _convertValues, value); }
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
#if !OFFLINE
                if (_iconPath == PyxisConstants.DummyIcon)
                {
                    _pixivUser?.ShowThumbnail();
                    _isRequested = true;
                }
#endif
                return _iconPath;
            }
            set
            {
                if (SetProperty(ref _iconPath, value) && _isRequested)
                    _pixivUser?.ShowThumbnail();
            }
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