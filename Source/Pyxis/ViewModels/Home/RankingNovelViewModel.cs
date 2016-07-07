using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Base;

namespace Pyxis.ViewModels.Home
{
    public class RankingNovelViewModel : RankingViewModel
    {
        public RankingNovelViewModel(INovel novel, RankingMode mode, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
            : base(mode, imageStoreService, navigationService)
        {
            Title = novel.Title;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivNovel(novel, imageStoreService);
        }
    }
}