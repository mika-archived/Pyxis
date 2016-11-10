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
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels
{
    public class FavoriteMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IDialogService _dialogService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private FavoriteOptionParameter _favoriteOption;
        private PixivFavorite _pixivFavorite;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> FavoriteItems { get; }

        public FavoriteMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                         IDialogService dialogService, IImageStoreService imageStoreService,
                                         INavigationService navigationService, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _dialogService = dialogService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            NavigationService = navigationService;
            FavoriteItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<FavoriteOptionParameter>((string) e?.Parameter);
            if (_accountService.IsLoggedIn)
                Initialize(parameter);
            else
                RunHelper.RunLaterUI(RedirectToLoginPage, parameter, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        #region Initializers

        private void Initialize(FavoriteOptionParameter parameter)
        {
            _categoryService.UpdateCategory();
            _favoriteOption = parameter;
            _favoriteOption.UserId = _accountService.LoggedInAccount.Id;
            SelectedIndex = (int) parameter.Type;
            _pixivFavorite = new PixivFavorite(_pixivClient, this._queryCacheService);

            if (parameter.Type == SearchType.IllustsAndManga)
                ModelHelper.ConnectTo(FavoriteItems, _pixivFavorite, w => w.ResultIllusts, CreatePixivImage).AddTo(this);
            else
                ModelHelper.ConnectTo(FavoriteItems, _pixivFavorite, w => w.ResultNovels, CreatePixivNovel).AddTo(this);

            Sync();
        }

        private void RedirectToLoginPage(FavoriteOptionParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "FavoriteMain", Parameter = parameter};
            NavigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #endregion

        #region Events

        private void Sync()
        {
            ParameterQueries = ParamGen.GenerateRaw(_favoriteOption, w => w.Type).Cast<object>().ToList();
            _pixivFavorite.Query(_favoriteOption);
        }

        public async void OnButtonTapped()
        {
            var result = await _dialogService.ShowDialogAsync("Dialogs.FavoriteOption", _favoriteOption);
            if (result == null)
                return;
            _favoriteOption = result as FavoriteOptionParameter;
            Sync();
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
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

        #region ParameterQueries

        private List<object> _parameterQueries;

        public List<object> ParameterQueries
        {
            get { return _parameterQueries; }
            set { SetProperty(ref _parameterQueries, value); }
        }

        #endregion
    }
}