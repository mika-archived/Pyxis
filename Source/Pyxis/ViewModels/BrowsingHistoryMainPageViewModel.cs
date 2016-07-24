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
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels
{
    public class BrowsingHistoryMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivBrowsingHistory _pixivBrowsingHistory;

        public INavigationService NavigationService { get; }
        public IncrementalObservableCollection<TappableThumbnailViewModel> BrowsingItems { get; }

        public BrowsingHistoryMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                                IImageStoreService imageStoreService,
                                                INavigationService navigationService,
                                                IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            BrowsingItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<BrowsingHistoryParameter>((string) e?.Parameter);
            if (!_accountService.IsLoggedIn)
                RunHelper.RunLater(RedirectToLoginPage, parameter, TimeSpan.FromMilliseconds(10));
            else if (!_accountService.IsPremium)
                RunHelper.RunLater(RedirectToPremiumErrorPage, parameter, TimeSpan.FromMilliseconds(10));
            else
                Initialize(parameter);
        }

        #endregion

        #region Initializers

        private void Initialize(BrowsingHistoryParameter parameter)
        {
            _categoryService.UpdateCategory();
            SelectedIndex = (int) parameter.ContentType;
            _pixivBrowsingHistory = new PixivBrowsingHistory(_pixivClient, parameter.ContentType);

            if (parameter.ContentType == ContentType2.IllustAndManga)
                ModelHelper.ConnectTo(BrowsingItems, _pixivBrowsingHistory, w => w.Illusts, CreatePixivImage)
                           .AddTo(this);
            else
                ModelHelper.ConnectTo(BrowsingItems, _pixivBrowsingHistory, w => w.Novels, CreatePixivNovel)
                           .AddTo(this);
        }

        private void RedirectToLoginPage(BrowsingHistoryParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "BrowsingHistoryMain", Parameter = parameter};
            NavigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        private void RedirectToPremiumErrorPage(BrowsingHistoryParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "BrowsingHistoryMain", Parameter = parameter};
            NavigationService.Navigate("Error.PremiumRequired", param.ToJson());
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
    }
}