using System;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Home.Items
{
    public class RankingNovelViewModel : ThumbnailableViewModel
    {
        private readonly RankingMode _mode;
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;

        public string Title => _novel.Title;
        public string Category => _mode.ToDisplayString();
        public ReactiveCommand ItemTappedCommand { get; }

        public RankingNovelViewModel(RankingMode mode, INovel novel, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
        {
            _mode = mode;
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