using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Base;

namespace Pyxis.ViewModels.Home
{
    public class RankingImageViewModel : RankingViewModel
    {
        public RankingImageViewModel(IIllust illust, RankingMode mode, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
            : base(mode, imageStoreService, navigationService)
        {
            Title = illust.Title;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivImage(illust, imageStoreService);
        }
    }
}