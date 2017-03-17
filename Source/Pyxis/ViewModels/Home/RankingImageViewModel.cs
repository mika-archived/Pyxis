using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Home
{
    public class RankingImageViewModel : RankingViewModel
    {
        private readonly ContentType _contentType;
        private readonly Illust _illust;

        public RankingImageViewModel(Illust illust, RankingMode mode, ContentType contentType, IImageStoreService imageStoreService,
                                     INavigationService navigationService)
            : base(mode, imageStoreService, navigationService)
        {
            _illust = illust;
            _contentType = contentType;
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
                RankingType = _contentType
            };
            NavigationService.Navigate($"Ranking.{_contentType.ToString().FirstCharToUpper()}Ranking",
                                       parameter.ToJson());
        }

        #endregion
    }
}