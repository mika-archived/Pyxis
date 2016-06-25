using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class NovelDetailPageViewModel : ThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;

        private int _count;
        private INovel _novel;
        private PixivComment _pixivComment;
        private PixivUser _pixivUser;

        public int Height => 221;
        public int Width => 176;

        public ObservableCollection<PixivTagViewModel> Tags { get; }
        public ObservableCollection<PixivCommentViewModel> Comments { get; }

        public NovelDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                        INavigationService navigationService, IPixivClient pixivClient)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            Tags = new ObservableCollection<PixivTagViewModel>();
            Comments = new ObservableCollection<PixivCommentViewModel>();
            ThumbnailPath = PyxisConstants.DummyImage;
            IconPath = PyxisConstants.DummyIcon;
        }

        private void Initialize(NovelDetailParameter parameter)
        {
            _count = 0;
            _novel = parameter.Novel;
            Title = _novel.Title;
            Description = _novel.Caption.Replace("<br />", Environment.NewLine);
            CreatedAt = _novel.CreateDate.ToString("g");
            Username = _novel.User.Name;
            View = _novel.TotalView;
            Bookmark = _novel.TotalBookmarks;
            TextLength = $"{_novel.TextLength.ToString("##,###")}文字";
            _novel.Tags.ForEach(w => Tags.Add(new PixivTagViewModel(w, _navigationService)));
            Thumbnailable = new PixivNovel(_novel, _imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
            _pixivUser = new PixivUser(_novel.User, _imageStoreService);
            _pixivUser.ObserveProperty(w => w.ThumbnailPath)
                      .Where(w => !string.IsNullOrWhiteSpace(w))
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => IconPath = w)
                      .AddTo(this);
            _pixivComment = new PixivComment(_novel, _pixivClient);
            _pixivComment.Comments.ObserveAddChanged()
                         .Where(w => ++_count <= 5)
                         .Select(CreatePixivComment)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => Comments.Add(w))
                         .AddTo(this);
        }

        public void OnTappedButton()
        {
            var parameter = new NovelDetailParameter {Novel = _novel};
            _navigationService.Navigate("Detail.NovelView", parameter.ToJson());
        }

        #region Converters

        private PixivCommentViewModel CreatePixivComment(IComment w) =>
            new PixivCommentViewModel(w, _imageStoreService);

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<NovelDetailParameter>((string) e.Parameter);
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

        #region TextLength

        private string _textLength;

        public string TextLength
        {
            get { return _textLength; }
            set { SetProperty(ref _textLength, value); }
        }

        #endregion
    }
}