using System;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivNovelViewModel : TappableThumbnailViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;

        public string Title => _novel.Title;
        public string Caption => _novel.Caption;
        public string CreatedAt => _novel.CreateDate.ToString("g");
        public int BookmarkCount => _novel.TotalBookmarks;
        public int ViewCount => _novel.TotalView;

        public PixivNovelViewModel(INovel novel, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _novel = novel;
            _navigationService = navigationService;

            Thumbnailable = new PixivNovel(novel, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }
    }
}