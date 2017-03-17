using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Home
{
    public class RankingNovelViewModel : RankingViewModel
    {
        public RankingNovelViewModel(Novel novel, RankingMode mode, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
            : base(mode, imageStoreService, navigationService)
        {
            Title = novel.Title;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivNovel(novel, imageStoreService);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new RankingParameter
            {
                RankingMode = RankingMode,
                RankingType = ContentType.Novel
            };
            NavigationService.Navigate("Ranking.NovelRanking", parameter.ToJson());
        }

        #endregion
    }
}