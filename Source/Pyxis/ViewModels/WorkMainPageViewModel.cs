using System;
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

namespace Pyxis.ViewModels
{
    public class WorkMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivWork _pixivWork;

        public INavigationService NavigationService { get; }
        public IncrementalObservableCollection<TappableThumbnailViewModel> WorkItems { get; }

        public WorkMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                     IImageStoreService imageStoreService, INavigationService navigationService,
                                     PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            WorkItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<WorkParameter>((string) e?.Parameter);
            if (_accountService.IsLoggedIn)
                Initialize(parameter);
            else
                RunHelper.RunLaterUI(RedirectToLoginPage, parameter, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        #region Initializers

        private void Initialize(WorkParameter parameter)
        {
            _categoryService.UpdateCategory();
            SelectedIndex = (int) parameter.ContentType;
            _pixivWork = new PixivWork(_accountService.LoggedInAccount.Id, parameter.ContentType, _pixivClient, _queryCacheService);
            if (parameter.ContentType != ContentType.Novel)
                ModelHelper.ConnectTo(WorkItems, _pixivWork, w => w.IllustsRoot, CreatePixivImage).AddTo(this);
            else
                ModelHelper.ConnectTo(WorkItems, _pixivWork, w => w.Novels, CreatePixivNovel).AddTo(this);
        }

        private void RedirectToLoginPage(WorkParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "WorkMain", Parameter = parameter};
            NavigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(Illust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(Novel w) =>
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