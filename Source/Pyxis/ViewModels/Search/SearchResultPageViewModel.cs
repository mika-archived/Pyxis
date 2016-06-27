using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels.Search
{
    public class SearchResultPageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivSearch _pixivSearch;
        private SearchOptionParameter _searchOption;

        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<ThumbnailableViewModel> Results { get; }

        public SearchResultPageViewModel(IImageStoreService imageStoreService, INavigationService navigationService,
                                         IPixivClient pixivClient)
        {
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            Results = new IncrementalObservableCollection<ThumbnailableViewModel>();
        }

        private void Initialize(SearchResultParameter parameter)
        {
            SelectedIndex = (int) parameter.Sort;
            SearchQuery = parameter.Query;
            _searchOption = new SearchOptionParameter
            {
                SearchType = parameter.SearchType,
                Sort = parameter.Sort,
                Target = parameter.Target,
                Duration = parameter.Duration,
                DurationQuery = parameter.DurationQuery
            };

            _pixivSearch = new PixivSearch(_pixivClient);
            if (parameter.SearchType == SearchType.IllustsAndManga)
                ModelHelper.ConnectTo(Results, _pixivSearch, w => w.ResultIllusts, CreatePixivImage);
            else if (parameter.SearchType == SearchType.Novels)
                ModelHelper.ConnectTo(Results, _pixivSearch, w => w.ResultNovels, CreatePixivNovel);

            Search();
        }

        private void GenerateQueries()
        {
            var param1 = SearchResultParameter.Build(_searchOption);
            param1.Sort = SearchSort.New;

            var param2 = SearchResultParameter.Build(_searchOption);
            param2.Sort = SearchSort.Popular;

            var param3 = SearchResultParameter.Build(_searchOption);
            param3.Sort = SearchSort.Old;

            param1.Query = param2.Query = param3.Query = SearchQuery;

            ParameterQueries = new List<string>
            {
                (string) param1.ToJson(),
                (string) param2.ToJson(),
                (string) param3.ToJson()
            };
        }

        public void OnQuerySubmitted() => Search();

        private void Search()
        {
            GenerateQueries();
            _pixivSearch.Search(SearchQuery, _searchOption);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<SearchResultParameter>((string) e?.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

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

        #region ParameterQuery

        // あんまり持ちたくない.
        private List<string> _parameterQueries;

        public List<string> ParameterQueries
        {
            get { return _parameterQueries; }
            set { SetProperty(ref _parameterQueries, value); }
        }

        #endregion
    }
}