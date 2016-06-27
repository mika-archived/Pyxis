﻿using System.Collections.Generic;

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

            _pixivSearch = new PixivSearch(_pixivClient, parameter.SearchType);
            if (parameter.SearchType == SearchType.IllustsAndManga)
                ModelHelper.ConnectTo(Results, _pixivSearch, w => w.ResultIllusts, CreatePixivImage);
            else if (parameter.SearchType == SearchType.Novels)
                ModelHelper.ConnectTo(Results, _pixivSearch, w => w.ResultNovels, CreatePixivNovel);

            Search();
        }

        private void GenerateQueries(SearchResultParameter parameter)
        {
            ParameterQueries = new List<string>();
            parameter.Sort = SearchSort.New;
        }

        public void OnQuerySubmitted() => Search();

        private void Search()
        {
            _pixivSearch.Search(SearchQuery);
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