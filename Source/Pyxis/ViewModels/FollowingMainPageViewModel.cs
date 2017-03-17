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
    public class FollowingMainPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivFollow _pixivFollow;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> FollowingUsers { get; }

        public FollowingMainPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                          IImageStoreService imageStoreService, INavigationService navigationService,
                                          PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            FollowingUsers = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<FollowingParameter>((string) e.Parameter);
            if (_accountService.IsLoggedIn)
                Initialize(parameter);
            else
                RunHelper.RunLaterUI(RedirectToLoginPage, parameter, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        #region Converters

        private TappableThumbnailViewModel CreateUserViewModel(UserPreview userPreview)
            => new UserCardViewModel(userPreview, _imageStoreService, NavigationService, _pixivClient);

        #endregion

        #region Initializers

        private void Initialize(FollowingParameter parameter)
        {
            _categoryService.UpdateCategory();
            SelectedIndex = (int) parameter.Restrict - 1;
            _pixivFollow = new PixivFollow(_accountService.LoggedInAccount.Id, parameter.Restrict, _pixivClient, _queryCacheService);
            ModelHelper.ConnectTo(FollowingUsers, _pixivFollow, w => w.Users, CreateUserViewModel).AddTo(this);
        }

        private void RedirectToLoginPage(FollowingParameter parameter)
        {
            var param = new RedirectParameter {RedirectTo = "FollowingMain", Parameter = parameter};
            NavigationService.Navigate("Error.LoginRequired", param.ToJson());
        }

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