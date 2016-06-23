using Prism.Windows.Navigation;

using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Home.Base
{
    public class RankingViewModel : TappableThumbnailViewModel
    {
        protected RankingMode RankingMode { get; }
        protected INavigationService NavigationService { get; }
        protected IImageStoreService ImageStoreService { get; }

        public string Title { get; protected set; }
        public string Category => RankingMode.ToDisplayString();

        public RankingViewModel(RankingMode mode, IImageStoreService imageStoreService,
                                INavigationService navigationService)
        {
            RankingMode = mode;
            ImageStoreService = imageStoreService;
            NavigationService = navigationService;
        }
    }
}