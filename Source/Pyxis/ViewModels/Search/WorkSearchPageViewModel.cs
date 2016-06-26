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
        private readonly IDialogService _dialogService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private int _count;
        private PixivTrending _pixivTrending;
        public INavigationService NavigationService { get; }
        public ObservableCollection<TrendingTagViewModel> TrendingTags { get; }

        public WorkSearchPageViewModel(IDialogService dialogService, IImageStoreService imageStoreService,
                                       INavigationService navigationService,
                                       IPixivClient pixivClient)
        {
            _dialogService = dialogService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            TrendingTags = new ObservableCollection<TrendingTagViewModel>();
        }

        private void Initialize(SearchHomeParameter parameter)
        {
            _count = 0;
            SelectedIndex = (int) parameter.SearchType;
            _pixivTrending = new PixivTrending(parameter.SearchType, _pixivClient);
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
            _pixivTrending.Fetch();
        }

        public async void OnButtonTapped() => await _dialogService.ShowDialogAsync("Dialogs.SearchOption", null);

        public void OnQuerySubmitted()
        {
            var parameter = new SearchResultParameter
            {
                Query = SearchQuery,
                SearchType = (SearchType) SelectedIndex
            };
            NavigationService.Navigate("Search.SearchResult", parameter.ToJson());
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<SearchHomeParameter>((string) e.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Converters

        private TrendingTagViewModel CreateTrendingTag(ITrendTag trendTag)
            => new TrendingTagViewModel(trendTag, _imageStoreService, NavigationService);

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