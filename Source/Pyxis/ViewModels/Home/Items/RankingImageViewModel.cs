using System;
using System.Diagnostics;

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
    public class RankingImageViewModel : ViewModel
    {
        private readonly IIllust _illust;
        private readonly RankingMode _mode;
        private readonly INavigationService _navigationService;
        private readonly PixivImage _pixivImage;

        public string Title => _illust.Title;

        public string Category => _mode.ToDisplayString();
        public ReactiveCommand ItemTappedCommand { get; }

        public RankingImageViewModel(RankingMode mode, IIllust illust, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
        {
            _mode = mode;
            _illust = illust;
            _navigationService = navigationService;

            _pixivImage = new PixivImage(illust, imageStoreService);
            _pixivImage.ObserveProperty(w => w.ImagePath)
                       .ObserveOnUIDispatcher()
                       .Subscribe(w => ThumbnailPath = w)
                       .AddTo(this);
            ItemTappedCommand = new ReactiveCommand();
            ItemTappedCommand.Subscribe(w => Debug.WriteLine(w)).AddTo(this);
        }

        #region ThumbnailPath

        private string _thumbnailPath;

        public string ThumbnailPath
        {
            get
            {
                if (_thumbnailPath == PyxisConstants.DummyImage)
                    _pixivImage.ShowThumbnail();
                return _thumbnailPath;
            }
            set { SetProperty(ref _thumbnailPath, value); }
        }

        #endregion
    }
}