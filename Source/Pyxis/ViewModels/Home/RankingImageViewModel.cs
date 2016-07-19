using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Base;

namespace Pyxis.ViewModels.Home
{
    public class RankingImageViewModel : RankingViewModel
    {
        private readonly IIllust _illust;

        public RankingImageViewModel(IIllust illust, RankingMode mode, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
            : base(mode, imageStoreService, navigationService)
        {
            _illust = illust;
            Title = _illust.Title;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivImage(_illust, imageStoreService);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new RankingParameter
            {
                RankingMode = RankingMode,
                RankingType = _illust.Type == "manga" ? ContentType.Manga : ContentType.Illust
            };
            NavigationService.Navigate("Ranking.IllustRanking", parameter.ToJson());
        }

        #endregion
    }
}