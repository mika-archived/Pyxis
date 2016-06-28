﻿using System;
using System.Collections.Generic;

using Windows.System;

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
        private readonly IAccountService _accountService;
        private readonly IDialogService _dialogService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivSearch _pixivSearch;
        private SearchOptionParameter _searchOption;

        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<ThumbnailableViewModel> Results { get; }

        public SearchResultPageViewModel(IAccountService accountService, IDialogService dialogService,
                                         IImageStoreService imageStoreService, INavigationService navigationService,
                                         IPixivClient pixivClient)
        {
            _accountService = accountService;
            _dialogService = dialogService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            Results = new IncrementalObservableCollection<ThumbnailableViewModel>();
        }

        private void Initialize(SearchResultParameter parameter)
        {
            SelectedIndex = (int) parameter.Sort;
            SearchQuery = parameter.Query;
            IsLoggedInRequired = !_accountService.IsLoggedIn && parameter.Sort == SearchSort.Popular;
            IsPremiumRequired = !_accountService.IsPremium && parameter.Sort == SearchSort.Popular;
            _searchOption = new SearchOptionParameter
            {
                SearchType = parameter.SearchType,
                Sort = parameter.Sort,
                Target = parameter.Target,
                Duration = parameter.Duration
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

        public async void OnButtonTapped()
        {
            var result = await _dialogService.ShowDialogAsync("Dialogs.SearchOption", _searchOption);
            if (result == null)
                return;
            _searchOption = result as SearchOptionParameter;
            Search();
        }

        public void OnQuerySubmitted() => Search();

        private void Search()
        {
            GenerateQueries();
            if (IsLoggedInRequired || IsPremiumRequired)
                return;
            _pixivSearch.Search(SearchQuery, _searchOption);
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public async void OnRegisterPremiumButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("http://www.pixiv.net/premium.php"));

        public void OnLoginButtonTapped()
        {
            var sp = SearchResultParameter.Build(_searchOption);
            sp.Query = SearchQuery;
            var parameter = new RedirectParameter
            {
                RedirectTo = "Search.SearchResult",
                Parameter = sp
            };
            NavigationService.Navigate("Account.Login", parameter.ToJson());
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

        #region IsLoggedInRequired

        private bool _isLoggedInRequired;

        public bool IsLoggedInRequired
        {
            get { return _isLoggedInRequired; }
            set { SetProperty(ref _isLoggedInRequired, value); }
        }

        #endregion

        #region IsPremiumRequired

        private bool _isPremiumRequired;

        public bool IsPremiumRequired
        {
            get { return _isPremiumRequired; }
            set { SetProperty(ref _isPremiumRequired, value); }
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