using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Home;
using Pyxis.ViewModels.Home.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings;

namespace Pyxis.ViewModels
{
    public class HomeMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivRanking _pixivRanking;
        private PixivRecommended _pixivRecommended;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveCollection<RankingViewModel> TopRanking { get; private set; }
        public IncrementalObservableCollection<TappableThumbnailViewModel> RecommendedItems { get; }

        public HomeMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                     IImageStoreService imageStoreService, IPixivClient pixivClient,
                                     INavigationService navigationService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            RecommendedItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameters = ParameterBase.ToObject<HomeParameter>(e.Parameter?.ToString());
            Initialize(parameters);
        }

        #endregion

        #region Initializers

        private void Initialize(HomeParameter parameter)
        {
            _categoryService.UpdateCategory();
            SelectedIndex = (int) parameter.ContentType;
            _pixivRanking = new PixivRanking(_pixivClient, parameter.ContentType);
            _pixivRecommended = new PixivRecommended(_accountService, _pixivClient, parameter.ContentType);

            if (parameter.ContentType == ContentType.Novel)
            {
                TopRanking = _pixivRanking.RankingOfNovels.ToReadOnlyReactiveCollection(CreateRankingNovel).AddTo(this);
                ModelHelper.ConnectTo(RecommendedItems, _pixivRecommended, w => w.RecommendedNovels, CreatePixivNovel);
            }
            else
            {
                TopRanking = _pixivRanking.Ranking.ToReadOnlyReactiveCollection(CreateRankingImage).AddTo(this);
                ModelHelper.ConnectTo(RecommendedItems, _pixivRecommended, w => w.RecommendedImages, CreatePixivImage);
            }
#if !OFFLINE
            _pixivRanking.Fetch();
#endif
        }

        #endregion

        #region Converters

        private RankingViewModel CreateRankingImage(Tuple<RankingMode, IIllusts> w) =>
            new RankingImageViewModel(w.Item2.IllustList.First(), w.Item1, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private RankingViewModel CreateRankingNovel(Tuple<RankingMode, INovels> w) =>
            new RankingNovelViewModel(w.Item2.NovelList.First(), w.Item1, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

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