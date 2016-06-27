using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels.Search.Items
{
    public class TrendingTagViewModel : PixivThumbnailViewModel
    {
        private readonly SearchType _searchType;
        private readonly ITrendTag _trendTag;

        public string TagName => _trendTag.Tag;

        public TrendingTagViewModel(SearchType searchType, ITrendTag trendTag, IImageStoreService imageStoreService,
                                    INavigationService navigationService)
            : base(trendTag.Illust, imageStoreService, navigationService)
        {
            _searchType = searchType;
            _trendTag = trendTag;
        }

        #region Overrides of PixivThumbnailViewModel

        public override void OnItemTapped()
        {
            if (Illust != null)
            {
                var param = new SearchResultParameter
                {
                    SearchType = _searchType,
                    Query = TagName,
                    Duration = SearchDuration.Nothing,
                    Sort = SearchSort.New,
                    Target = SearchTarget.TagTotal
                };
                NavigationService.Navigate("Search.SearchResult", param.ToJson());
            }
        }

        #endregion
    }
}