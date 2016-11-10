using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Search.Items;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Search
{
    public class WorkSearchPageViewModel : ViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IDialogService _dialogService;
        private readonly IImageStoreService _imageStoreService;
        private readonly ILicenseService _licenseService;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private int _count;
        private PixivTrending _pixivTrending;
        private SearchOptionParameter _searchOption;
        public INavigationService NavigationService { get; }
        public ObservableCollection<TrendingTagViewModel> TrendingTags { get; }

        public bool IsActivatedAdvancedSearch => _licenseService.IsActivated("AdvancedSearch");

        public WorkSearchPageViewModel(ICategoryService categoryService, IDialogService dialogService,
                                       IImageStoreService imageStoreService, ILicenseService licenseService,
                                       INavigationService navigationService, IPixivClient pixivClient,
                                       IQueryCacheService queryCacheService)
        {
            _categoryService = categoryService;
            _dialogService = dialogService;
            _imageStoreService = imageStoreService;
            _licenseService = licenseService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            TrendingTags = new ObservableCollection<TrendingTagViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<SearchHomeParameter>((string) e.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Initializers

        private void Initialize(SearchHomeParameter parameter)
        {
            _categoryService.UpdateCategory();
            _count = 0;
            _searchOption = new SearchOptionParameter
            {
                SearchType = parameter.SearchType,
                Target = SearchTarget.TagPartial,
                Duration = SearchDuration.Nothing
            };
            SelectedIndex = (int) parameter.SearchType;
            _pixivTrending = new PixivTrending(parameter.SearchType, _pixivClient, _queryCacheService);
            var observable = _pixivTrending.TrendingTags.ObserveAddChanged().Do(w => ++_count).Publish();
            observable.Where(w => _count <= 1)
                      .Select(CreateTrendingTag)
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => TopTrendingTag = w)
                      .AddTo(this);
            observable.Where(w => _count > 1)
                      .Select(CreateTrendingTag)
                      .ObserveOnUIDispatcher()
                      .Subscribe(w => TrendingTags.Add(w))
                      .AddTo(this);
            observable.Connect().AddTo(this);
#if !OFFLINE
            _pixivTrending.Fetch();
#endif
        }

        #endregion

        #region Converters

        private TrendingTagViewModel CreateTrendingTag(ITrendTag trendTag)
            => new TrendingTagViewModel((SearchType) SelectedIndex, trendTag, _imageStoreService, NavigationService);

        #endregion

        #region Events

        public async void OnSearchOptionButtonTapped()
        {
            var result = await _dialogService.ShowDialogAsync("Dialogs.SearchOption", _searchOption);
            if (result != null)
                _searchOption = result as SearchOptionParameter;
        }

        public async void OnAdvancedSearchOptionButtonTapped()
        {
            var result = await _dialogService.ShowDialogAsync("Dialogs.AdvancedSearchOption", _searchOption);
            if (result != null)
                _searchOption = result as SearchOptionParameter;
        }

        public void OnQuerySubmitted()
        {
            var parameter = new SearchResultParameter
            {
                Query = SearchQuery,
                SearchType = _searchOption.SearchType,
                Sort = _searchOption.Sort,
                Duration = _searchOption.Duration,
                Target = _searchOption.Target,
                EitherWord = _searchOption.EitherWord,
                IgnoreWord = _searchOption.IgnoreWord,
                BookmarkCount = _searchOption.BookmarkCount,
                ViewCount = _searchOption.ViewCount,
                CommentCount = _searchOption.CommentCount,
                PageCount = _searchOption.PageCount,
                Height = _searchOption.Height,
                Width = _searchOption.Width,
                Tool = _searchOption.Tool,
                TextLength = _searchOption.TextLength
            };
            NavigationService.Navigate("Search.SearchResult", parameter.ToJson());
        }

        #endregion

        #region TopTrendingTag

        private TrendingTagViewModel _topTrendingTag;

        public TrendingTagViewModel TopTrendingTag
        {
            get { return _topTrendingTag; }
            set { SetProperty(ref _topTrendingTag, value); }
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