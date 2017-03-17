using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels.New
{
    public class AllNewPageViewModel : ViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivNew _pixivNew;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> NewItems { get; }

        public AllNewPageViewModel(IImageStoreService imageStoreService, ICategoryService categoryService,
                                   INavigationService navigationService, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            NavigationService = navigationService;
            NewItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
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
            SubSelectdIndex = (int)parameter.ContentType;

            _pixivNew = new PixivNew(parameter.ContentType, FollowType.All, _pixivClient, _queryCacheService);
            if (parameter.ContentType == ContentType.Novel)
                ModelHelper.ConnectTo(NewItems, _pixivNew, w => w.NewNovels, CreatePixivNovel).AddTo(this);
            else
                ModelHelper.ConnectTo(NewItems, _pixivNew, w => w.NewIllustsRoot, CreatePixivImage).AddTo(this);
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(Illust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(Novel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        #endregion

        #region SubSelectdIndex

        private int _subSelectedIndex;

        public int SubSelectdIndex
        {
            get { return _subSelectedIndex; }
            set { SetProperty(ref _subSelectedIndex, value); }
        }

        #endregion
    }
}