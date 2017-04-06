using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Microsoft.Toolkit.Uwp;

using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;
using Pyxis.ViewModels.Partials;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta;
using Sagitta.Models;

using WinRTXamlToolkit.Tools;

namespace Pyxis.ViewModels
{
    public class IllustPageViewModel : ViewModel
    {
        private readonly PixivPostDetail<Illust> _postDetail;

        public ReadOnlyReactiveProperty<Uri> AuthorIconUrl { get; }
        public ReadOnlyReactiveProperty<string> AuthorName { get; }
        public ReadOnlyReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveProperty<string> ThumbnailUrl { get; }
        public ReadOnlyReactiveProperty<Uri> OriginalUrl { get; }
        public ReadOnlyReactiveProperty<bool> HasMultiPage { get; }
        public ReadOnlyReactiveProperty<int> PageCount { get; }
        public ReadOnlyReactiveProperty<int> MaxHeight { get; }
        public ReadOnlyReactiveProperty<int> MaxWidth { get; }
        public ReadOnlyReactiveProperty<string> CreatedAt { get; }
        public ReadOnlyReactiveProperty<string> Views { get; }
        public ReadOnlyReactiveProperty<string> Bookmarks { get; }
        public ReadOnlyReactiveProperty<bool> HasComments { get; }
        public ObservableCollection<TagViewModel> Tags { get; }
        public ReadOnlyReactiveProperty<string> Description { get; }
        public IncrementalLoadingCollection<PixivRelatedSource<IllustViewModel>, IllustViewModel> RelatedSource { get; }

        public IllustPageViewModel(PixivClient pixivClient, IFileCacheService cacheService)
        {
            _postDetail = new PixivPostDetail<Illust>(pixivClient);
            Tags = new ObservableCollection<TagViewModel>();
            var relatedSource = new PixivRelatedSource<IllustViewModel>(pixivClient);
            RelatedSource = new IncrementalLoadingCollection<PixivRelatedSource<IllustViewModel>, IllustViewModel>(relatedSource);
            var comment = new PixivComment(pixivClient);
            var connector = _postDetail.ObserveProperty(w => w.Post).Where(w => w != null).Publish();
            AuthorIconUrl = connector.Select(w => new Uri(w.User.ProfileImageUrls.Medium)).ToReadOnlyReactiveProperty().AddTo(this);
            AuthorName = connector.Select(w => w.User.Name).ToReadOnlyReactiveProperty().AddTo(this);
            Title = connector.Select(w => $"{w.Title}").ToReadOnlyReactiveProperty().AddTo(this);
            ThumbnailUrl = connector.SelectMany(async w =>
            {
                if (await cacheService.ExistFileAsync(w.ImageUrls.SquareMedium))
                    return await cacheService.LoadFileAsync(w.ImageUrls.SquareMedium);
                return PyxisConstants.PlaceholderSquare;
            }).ToReadOnlyReactiveProperty(PyxisConstants.PlaceholderSquare).AddTo(this);
            OriginalUrl = connector.Select(w => new Uri(w.ImageUrls.Large))
                                   .ToReadOnlyReactiveProperty()
                                   .AddTo(this);
            HasMultiPage = connector.Select(w => w.PageCount > 1).ToReadOnlyReactiveProperty().AddTo(this);
            PageCount = connector.Select(w => w.PageCount).ToReadOnlyReactiveProperty().AddTo(this);
            MaxHeight = connector.Select(w => w.Height).ToReadOnlyReactiveProperty().AddTo(this);
            MaxWidth = connector.Select(w => w.Width).ToReadOnlyReactiveProperty().AddTo(this);
            CreatedAt = connector.Select(w => w.CreatedAt.ToString("d")).ToReadOnlyReactiveProperty().AddTo(this);
            Views = connector.Select(w => $"{w.TotalView:##,###} 閲覧").ToReadOnlyReactiveProperty().AddTo(this);
            Bookmarks = connector.Select(w => $"{w.TotalBookmarks:##,###} ブックマーク").ToReadOnlyReactiveProperty().AddTo(this);
            connector.Select(w => w.Tags).Subscribe(w =>
            {
                Tags.Clear();
                w.ForEach(v => Tags.Add(new TagViewModel(v)));
            });
            Description = connector.Select(w => w.Caption).ToReadOnlyReactiveProperty().AddTo(this);
            connector.Subscribe(async w =>
            {
                CommentsAreaViewModel = new CommentsAreaViewModel(w, comment);
                await comment.FetchCommentAsync(w);
                relatedSource.Apply(w, v => new IllustViewModel(v));
                await RelatedSource.RefreshAsync();
            });
            connector.Connect().AddTo(this);

            var commentConnector = comment.ObserveProperty(w => w.TotalComments).Publish();
            HasComments = commentConnector.Select(w => w > 0).ToReadOnlyReactiveProperty().AddTo(this);
            commentConnector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<PostParameter<Illust>>();
            if (parameter.HasObject)
                _postDetail.ApplyForce(parameter.Post);
            else
                RunHelper.RunAsync(() => _postDetail.FetchAsync(parameter.Id));
        }

        public void OnImageTapped()
        {
            NavigateTo(_postDetail.Post.PageCount == 1 ? "Viewers.IllustViewer" : "Viewers.MangaViewer", new PostParameter<Illust> {Post = _postDetail.Post});
        }

        #region CommentsAreaViewModel

        private CommentsAreaViewModel _commentsAreaViewModel;

        public CommentsAreaViewModel CommentsAreaViewModel
        {
            get => _commentsAreaViewModel;
            set => SetProperty(ref _commentsAreaViewModel, value);
        }

        #endregion
    }
}