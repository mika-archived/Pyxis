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

namespace Pyxis.ViewModels.Search
{
    public class UserSearchPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivRecommended _pixivRecommended;
        private PixivSearch _pixivSearch;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> Users { get; }

        public UserSearchPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                       IImageStoreService imageStoreService, INavigationService navigationService,
                                       IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            Users = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Initialize();
        }

        #endregion

        #region Initializers

        private void Initialize()
        {
            _categoryService.UpdateCategory();
            _pixivRecommended = new PixivRecommended(_accountService, _pixivClient, ContentType.User);
            _pixivSearch = new PixivSearch(_pixivClient);
            ModelHelper.ConnectTo(Users, _pixivRecommended, w => w.RecommendedUsers, CreateUserViewModel);
        }

        #endregion

        #region Events

        public void OnQuerySubmitted()
        {
            if (_pixivRecommended != null)
            {
                _pixivRecommended = null;
                Users.Clear();
                ModelHelper.ConnectTo(Users, _pixivSearch, w => w.ResultUsers, CreateUserViewModel);
            }
            _pixivSearch.Search(QueryText, new SearchOptionParameter {SearchType = SearchType.Users});
        }

        #endregion

        #region Converters

        private TappableThumbnailViewModel CreateUserViewModel(IUserPreview userPreview)
            => new UserCardViewModel(userPreview, _imageStoreService, NavigationService, _pixivClient);

        #endregion

        #region QueryText

        private string _queryText;

        public string QueryText
        {
            get { return _queryText; }
            set { SetProperty(ref _queryText, value); }
        }

        #endregion
    }
}