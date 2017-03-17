using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels.Ranking
{
    public class IllustRankingPageViewModel : ViewModel

    {
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivRanking _pixivRanking;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> RankingItems { get; }

        public IllustRankingPageViewModel(ICategoryService categoryService, IImageStoreService imageStoreService,
                                          INavigationService navigationService, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            RankingItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<RankingParameter>((string) e.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Initializers

        private void Initialize(RankingParameter parameter)
        {
            _categoryService.UpdateCategory();
            SelectedIndex = (int) parameter.RankingMode;
            _pixivRanking = new PixivRanking(_pixivClient, parameter.RankingType, parameter.RankingMode, _queryCacheService);
            ModelHelper.ConnectTo(RankingItems, _pixivRanking, w => w.Illusts, CreatePixivImage);
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(Illust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        #endregion

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion
    }
}