using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels.Search.Items
{
    public class TrendingTagViewModel : PixivThumbnailViewModel
    {
        private readonly ITrendTag _trendTag;

        public string TagName => _trendTag.Tag;

        public TrendingTagViewModel(ITrendTag trendTag, IImageStoreService imageStoreService,
                                    INavigationService navigationService)
            : base(trendTag.Illust, imageStoreService, navigationService)
        {
            _trendTag = trendTag;
        }

        #region Overrides of PixivThumbnailViewModel

        public override void OnItemTapped()
        {
            if (Illust != null)
            {
                // NavigationService.Navigate("", null);
            }
        }

        #endregion
    }
}