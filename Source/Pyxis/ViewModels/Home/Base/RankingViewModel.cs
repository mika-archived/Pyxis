using System;

using Prism.Windows.Navigation;

using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;

namespace Pyxis.ViewModels.Home.Base
{
    public class RankingViewModel : ThumbnailableViewModel
    {
        protected RankingMode RankingMode { get; }
        protected INavigationService NavigationService { get; }
        protected IImageStoreService ImageStoreService { get; }

        public string Title { get; protected set; }
        public string Category => RankingMode.ToDisplayString();

        public ReactiveCommand ItemTappedCommand { get; }

        public RankingViewModel(RankingMode mode, IImageStoreService imageStoreService,
                                INavigationService navigationService)
        {
            RankingMode = mode;
            ImageStoreService = imageStoreService;
            NavigationService = navigationService;

            ItemTappedCommand = new ReactiveCommand();
            ItemTappedCommand.Subscribe(w => ItemTapped()).AddTo(this);
        }

        protected virtual void ItemTapped()
        {
            // Nothing to do.
        }
    }
}