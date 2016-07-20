using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Windows.Input;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Commands;
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
    public class IllustDetailPageViewModel : TappableThumbnailViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private int _count;
        private IIllust _illust;
        private PixivComment _pixivComment;
        private PixivDetail _pixivDetail;
        private PixivRelated _pixivRelated;
        private PixivUserImage _pixivUser;

        public ObservableCollection<PixivTagViewModel> Tags { get; }
        public ObservableCollection<PixivCommentViewModel> Comments { get; }
        public IncrementalObservableCollection<PixivThumbnailViewModel> RelatedItems { get; }

        public IllustDetailPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                         IImageStoreService imageStoreService, INavigationService navigationService,
                                         IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            Tags = new ObservableCollection<PixivTagViewModel>();
            Comments = new ObservableCollection<PixivCommentViewModel>();
            RelatedItems = new IncrementalObservableCollection<PixivThumbnailViewModel>();
            ThumbnailPath = PyxisConstants.DummyImage;
            IconPath = PyxisConstants.DummyIcon;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<IllustDetailParameter>((string) e.Parameter);
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

        #region Initializers

        private void Initialize()
        {
            _categoryService.UpdateCategory();
            Title = _illust.Title;
            ConvertValues = new List<object> {_illust.Caption, _navigationService};
            CreatedAt = _illust.CreateDate.ToString("g");
            Username = _illust.User.Name;
            View = _illust.TotalView;
            BookmarkCount = _illust.TotalBookmarks;
            Height = _illust.Height;
            Width = _illust.Width;
            IsManga = _illust.PageCount > 1;
            IsBookmarked = _illust.IsBookmarked;
            _illust.Tags.ForEach(w => Tags.Add(new PixivTagViewModel(w, _navigationService)));
            Thumbnailable = new PixivImage(_illust, _imageStoreService, true);
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
            ModelHelper.ConnectTo(RelatedItems, _pixivRelated, w => w.RelatedIllusts, CreatePixivImage).AddTo(this);
#if !OFFLINE
            if (IconPath == PyxisConstants.DummyIcon)
                RunHelper.RunLater(_pixivUser.ShowThumbnail, TimeSpan.FromMilliseconds(100));
#endif
        }

        private void Initialize(IllustDetailParameter parameter)
        {
            _count = 0;
            _illust = parameter.Illust;
            Initialize();
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
                            Initialize();
                        }).AddTo(this);
            _pixivDetail.Fetch();
        }

        #endregion

        #region Events

        public override void OnItemTapped()
        {
            var parameter = new IllustDetailParameter {Illust = _illust};
            var type = "Illust";
            if (IsManga)
                type = "Manga";
            _navigationService.Navigate($"Detail.{type}View", parameter.ToJson());
        }

        public void OnTappedUserIcon()
        {
            var parameter = new DetailByIdParameter {Id = _illust.User.Id};
            _navigationService.Navigate("Detail.UserDetail", parameter.ToJson());
        }

        #region BookmarkCommand

        private ICommand _bookmarkCommand;

        public ICommand BookmarkCommand
            => _bookmarkCommand ?? (_bookmarkCommand = new DelegateCommand(Bookmark, CanBookmark));

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async void Bookmark()
        {
            if (IsBookmarked)
                await _pixivClient.IllustV1.Bookmark.DeleteAsync(illust_id => _illust.Id, restrict => "public");
            else
                await _pixivClient.IllustV1.Bookmark.AddAsync(illust_id => _illust.Id, restrict => "public");
            IsBookmarked = !IsBookmarked;
        }

        private bool CanBookmark() => _accountService.IsLoggedIn;

        #endregion

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, _navigationService);

        private PixivCommentViewModel CreatePixivComment(IComment w) =>
            new PixivCommentViewModel(w, _imageStoreService);

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
                    _pixivUser?.ShowThumbnail();
#endif
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

        #region BookmarkCount

        private int _bookmarkCount;

        public int BookmarkCount
        {
            get { return _bookmarkCount; }
            set { SetProperty(ref _bookmarkCount, value); }
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

        #region IsManga

        private bool _isManga;

        public bool IsManga
        {
            get { return _isManga; }
            set { SetProperty(ref _isManga, value); }
        }

        #endregion

        #region IsBookmarked

        private bool _isBookmarked;

        public bool IsBookmarked
        {
            get { return _isBookmarked; }
            set { SetProperty(ref _isBookmarked, value); }
        }

        #endregion
    }
}