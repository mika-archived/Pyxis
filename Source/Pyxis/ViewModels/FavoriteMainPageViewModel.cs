using System;
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

namespace Pyxis.ViewModels
{
    public class FavoriteMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IDialogService _dialogService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private FavoriteOptionParameter _favoriteOption;
        private PixivFavorite _pixivFavorite;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> FavoriteItems { get; }

        public FavoriteMainPageViewModel(IAccountService accountService, IDialogService dialogService,
                                         IImageStoreService imageStoreService, IPixivClient pixivClient,
                                         INavigationService navigationService)
        {
            _accountService = accountService;
            _dialogService = dialogService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            FavoriteItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        private void Initialize(FavoriteOptionParameter parameter)
        {
            _favoriteOption = parameter;
            _favoriteOption.UserId = _accountService.LoggedInAccount.Id;
            SelectedIndex = (int) parameter.Type;
            _pixivFavorite = new PixivFavorite(_pixivClient);

            if (parameter.Type == SearchType.IllustsAndManga)
                ModelHelper.ConnectTo(FavoriteItems, _pixivFavorite, w => w.ResultIllusts, CreatePixivImage);
            else
                ModelHelper.ConnectTo(FavoriteItems, _pixivFavorite, w => w.ResultNovels, CreatePixivNovel);

            Sync();
        }

        private void GenerateQueries()
        {
            var param1 = _favoriteOption.Clone();
            param1.Type = SearchType.IllustsAndManga;

            var param2 = _favoriteOption.Clone();
            param2.Type = SearchType.Novels;

            ParameterQueries = new List<string>
            {
                (string) param1.ToJson(),
                (string) param2.ToJson()
            };
        }

        private void Sync()
        {
            GenerateQueries();
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

        private void RedirectoToLoginPage(FavoriteOptionParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "FavoriteMain", Parameter = parameter};
            NavigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<FavoriteOptionParameter>((string) e?.Parameter);
            if (_accountService.IsLoggedIn)
                Initialize(parameter);
            else
                RunHelper.RunLater(RedirectoToLoginPage, parameter, TimeSpan.FromMilliseconds(10));
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

        private List<string> _parameterQueries;

        public List<string> ParameterQueries
        {
            get { return _parameterQueries; }
            set { SetProperty(ref _parameterQueries, value); }
        }

        #endregion
    }
}