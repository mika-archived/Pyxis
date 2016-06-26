using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Search
{
    public class SearchResultPageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        public INavigationService NavigationService { get; }

        public SearchResultPageViewModel(IImageStoreService imageStoreService, INavigationService navigationService,
                                         IPixivClient pixivClient)
        {
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
        }

        private void Initialize(SearchResultParameter parameter)
        {
            SelectedIndex = (int) parameter.SearchType;
            SearchQuery = parameter.Query;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<SearchResultParameter>((string) e?.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region SearchQuery

        private string _searchQuery;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { SetProperty(ref _searchQuery, value); }
        }

        #endregion

        #region SelectdIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion
    }
}