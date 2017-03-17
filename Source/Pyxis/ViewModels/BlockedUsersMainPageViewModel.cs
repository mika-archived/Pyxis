using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels
{
    public class BlockedUsersMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivBlocking _pixivBlocking;
        public IncrementalObservableCollection<TappableThumbnailViewModel> BlockingUsers { get; }

        public BlockedUsersMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                             IImageStoreService imageStoreService, INavigationService navigationService,
                                             PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            BlockingUsers = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (_accountService.IsLoggedIn)
                Initialize();
            else
                RunHelper.RunLaterUI(RedirectToLoginPage, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        #region Converters

        private TappableThumbnailViewModel CreateUserViewModel(User user)
            => new UserCardViewModel(user, _imageStoreService, _navigationService, _pixivClient);

        #endregion

        #region Initializers

        private void Initialize()
        {
            _categoryService.UpdateCategory();
            _pixivBlocking = new PixivBlocking(_pixivClient, _queryCacheService);
            ModelHelper.ConnectTo(BlockingUsers, _pixivBlocking, w => w.Users, CreateUserViewModel).AddTo(this);
        }

        private void RedirectToLoginPage()
        {
            var param = new RedirectParameter { RedirectTo = "BlockedUsersMain", Parameter = null };
            _navigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #endregion
    }
}