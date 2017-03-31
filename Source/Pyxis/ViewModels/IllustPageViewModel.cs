using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

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
        public ReadOnlyReactiveProperty<int> MaxHeight { get; }
        public ReadOnlyReactiveProperty<int> MaxWidth { get; }
        public ReadOnlyReactiveProperty<string> CreatedAt { get; }
        public ReadOnlyReactiveProperty<string> Views { get; }
        public ReadOnlyReactiveProperty<string> Bookmarks { get; }
        public ObservableCollection<TagViewModel> Tags { get; }
        public ReadOnlyReactiveProperty<string> Description { get; }

        public IllustPageViewModel(PixivClient pixivClient, IFileCacheService cacheService)
        {
            _postDetail = new PixivPostDetail<Illust>(pixivClient);
            Tags = new ObservableCollection<TagViewModel>();

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
            OriginalUrl = connector.Select(w => new Uri(w.MetaSinglePage.OriginalImageUrl ?? w.MetaPages.First().ImageUrls.Original))
                                   .ToReadOnlyReactiveProperty()
                                   .AddTo(this);
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
            connector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<IllustParameter>();
            if (parameter.HasObject)
                _postDetail.ApplyForce(parameter.Illust);
            else
                RunHelper.RunAsync(() => _postDetail.FetchAsync(parameter.Id));
        }

        public void OnImageTapped()
        {
            NavigateTo(_postDetail.Post.PageCount == 1 ? "Viewers.IllustViewer" : "Viewers.MangaViewer", new IllustParameter {Illust = _postDetail.Post});
        }
    }
}