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
    public class FollowerMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivFollower _pixivFollower;
        public IncrementalObservableCollection<TappableThumbnailViewModel> FollowerUsers { get; }

        public FollowerMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                         IImageStoreService imageStoreService, INavigationService navigationService,
                                         PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            FollowerUsers = new IncrementalObservableCollection<TappableThumbnailViewModel>();
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

        private TappableThumbnailViewModel CreateUserViewModel(UserPreview userPreview)
            => new UserCardViewModel(userPreview, _imageStoreService, _navigationService, _pixivClient);

        #endregion

        #region Initializers

        private void Initialize()
        {
            _categoryService.UpdateCategory();
            _pixivFollower = new PixivFollower(_accountService.LoggedInAccount.Id, _pixivClient);
            ModelHelper.ConnectTo(FollowerUsers, _pixivFollower, w => w.Users, CreateUserViewModel).AddTo(this);
        }

        private void RedirectToLoginPage()
        {
            var param = new RedirectParameter {RedirectTo = "FollowerMain", Parameter = null};
            _navigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

        #endregion
    }
}