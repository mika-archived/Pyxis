using System;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Home.Items
{
    public class RankingNovelViewModel : ViewModel
    {
        private readonly RankingMode _mode;
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;
        private readonly PixivNovel _pixivNovel;

        public string Title => _novel.Title;
        public string Category => _mode.ToDisplayString();
        public ReactiveCommand ItemTappedCommand { get; }

        public RankingNovelViewModel(RankingMode mode, INovel novel, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
        {
            _mode = mode;
            _novel = novel;
            _navigationService = navigationService;

            _pixivNovel = new PixivNovel(novel, imageStoreService);
            _pixivNovel.ObserveProperty(w => w.ThumbnailPath).Subscribe(w => ThumbnailPath = w).AddTo(this);
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