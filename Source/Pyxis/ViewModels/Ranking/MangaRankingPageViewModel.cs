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

namespace Pyxis.ViewModels.Ranking
{
    public class MangaRankingPageViewModel : ViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivRanking _pixivRanking;
        public INavigationService NavigationService { get; }
        public IncrementalObservableCollection<TappableThumbnailViewModel> RankingItems { get; }

        public MangaRankingPageViewModel(ICategoryService categoryService, IImageStoreService imageStoreService,
                                         INavigationService navigationService, IPixivClient pixivClient)
        {
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
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
            SelectedIndex = parameter.RankingMode.ToParamIndex(parameter.RankingType);
            _pixivRanking = new PixivRanking(_pixivClient, parameter.RankingType, parameter.RankingMode);
            ModelHelper.ConnectTo(RankingItems, _pixivRanking, w => w.Illusts, CreatePixivImage);
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
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