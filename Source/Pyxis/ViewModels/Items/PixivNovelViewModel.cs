using System;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivNovelViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;
        private readonly PixivNovel _pixivNovel;

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

            _pixivNovel = new PixivNovel(novel, imageStoreService);
            _pixivNovel.ObserveProperty(w => w.ThumbnailPath)
                       .ObserveOnUIDispatcher()
                       .Subscribe(w => ThumbnailPath = w)
                       .AddTo(this);
        }

        #region ThumbnailPath

        private string _thumbnailPath;

        public string ThumbnailPath
        {
            get
            {
                if (_thumbnailPath == PyxisConstants.DummyImage)
                    _pixivNovel.ShowThumbnail();
                return _thumbnailPath;
            }
            set { SetProperty(ref _thumbnailPath, value); }
        }

        #endregion
    }
}