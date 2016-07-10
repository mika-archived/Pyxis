using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Search.Items;

namespace Pyxis.ViewModels.Search
{
    public class UserSearchPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivRecommended _pixivRecommended;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> RecommendedUsers { get; }

        public UserSearchPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                       INavigationService navigationService,
                                       IPixivClient pixivClient)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            RecommendedUsers = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        private void Initialize()
        {
            _pixivRecommended = new PixivRecommended(_accountService, _pixivClient, ContentType.User);
            ModelHelper.ConnectTo(RecommendedUsers, _pixivRecommended, w => w.RecommendedUsers, CreateUserViewModel);
        }

        public void OnQuerySubmitted()
        {

        }

        public void OnButtonTapped()
        {

        }

        #region Converters

        private TappableThumbnailViewModel CreateUserViewModel(IUserPreview userPreview)
            => new UserCardViewModel(userPreview, _imageStoreService, NavigationService, _pixivClient);

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Initialize();
        }

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