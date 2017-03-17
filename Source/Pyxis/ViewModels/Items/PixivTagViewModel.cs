using Prism.Windows.Navigation;

using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Items
{
    public class PixivTagViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly Tag _tag;

        public string Name => _tag.Name;

        public PixivTagViewModel(Tag tag, INavigationService navigationService)
        {
            _tag = tag;
            _navigationService = navigationService;
        }

        #region Events

        public void OnItemTapped()
        {
            var parameter = new SearchResultParameter
            {
                SearchType = SearchType.IllustsAndManga,
                Target = SearchTarget.TagTotal,
                Duration = SearchDuration.Nothing,
                Sort = SearchSort.New,
                Query = Name
            };
            _navigationService.Navigate("Search.SearchResult", parameter.ToJson());
        }

        #endregion
    }
}