using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

namespace Pyxis.ViewModels
{
    public class BookmarkMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private PixivBookmark _pixivBookmark;
        public IncrementalObservableCollection<TappableThumbnailViewModel> BookmarkItems { get; }

        public BookmarkMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                         IImageStoreService imageStoreService, INavigationService navigationService,
                                         IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            BookmarkItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (!_accountService.IsLoggedIn)
                RunHelper.RunLater(RedirectToLoginPage, TimeSpan.FromMilliseconds(10));
            else
                Initialize();
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, _navigationService);

        #endregion

        #region Initializer

        private void Initialize()
        {
            _categoryService.UpdateCategory();
            _pixivBookmark = new PixivBookmark(_pixivClient);
            ModelHelper.ConnectTo(BookmarkItems, _pixivBookmark, w => w.Novels, CreatePixivNovel);
        }

        private void RedirectToLoginPage()
        {
            var param = new RedirectParameter {RedirectTo = "BookmarkMain", Parameter = null};
            _navigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #endregion
    }
}