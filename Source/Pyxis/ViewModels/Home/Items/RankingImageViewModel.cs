using System;
using System.Diagnostics;

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
    public class RankingImageViewModel : ThumbnailableViewModel
    {
        private readonly IIllust _illust;
        private readonly RankingMode _mode;
        private readonly INavigationService _navigationService;

        public string Title => _illust.Title;

        public string Category => _mode.ToDisplayString();
        public ReactiveCommand ItemTappedCommand { get; }

        public RankingImageViewModel(RankingMode mode, IIllust illust, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
        {
            _mode = mode;
            _illust = illust;
            _navigationService = navigationService;

            Thumbnailable = new PixivImage(illust, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
            ItemTappedCommand = new ReactiveCommand();
            ItemTappedCommand.Subscribe(w => Debug.WriteLine(w)).AddTo(this);
        }
    }
}