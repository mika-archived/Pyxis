using System;
using System.Collections.Generic;

using Windows.System;

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

namespace Pyxis.ViewModels.New
{
    public class FollowMypixivNewPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivNew _pixivNew;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> NewItems { get; }

        public FollowMypixivNewPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                             IPixivClient pixivClient, INavigationService navigationService)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            NewItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        private void Initialize(NewParameter parameter)
        {
            SelectedIndex = (int) parameter.FollowType;
            SubSelectdIndex = (int) parameter.ContentType;
            IsLoggedInRequired = !_accountService.IsLoggedIn;

            if (IsLoggedInRequired)
                return;

            _pixivNew = new PixivNew(parameter.ContentType.Convert(), parameter.FollowType, _pixivClient);
            if (parameter.ContentType == ContentType2.IllustAndManga)
                ModelHelper.ConnectTo(NewItems, _pixivNew, w => w.NewIllusts, CreatePixivImage);
            else
                ModelHelper.ConnectTo(NewItems, _pixivNew, w => w.NewNovels, CreatePixivNovel);
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public void OnLoginButtonTapped()
        {
            var parameter = new RedirectParameter
            {
                RedirectTo = "New.FollowMypixivNew",
                Parameter = new NewParameter
                {
                    ContentType = (ContentType2) SubSelectdIndex,
                    FollowType = (FollowType) SelectedIndex
                }
            };
            NavigationService.Navigate("Account.Login", parameter.ToJson());
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameters = ParameterBase.ToObject<NewParameter>(e.Parameter?.ToString());
            Initialize(parameters);
        }

        #endregion

        #region IsLoggedInRequired

        private bool _isLoggedInRequired;

        public bool IsLoggedInRequired
        {
            get { return _isLoggedInRequired; }
            set { SetProperty(ref _isLoggedInRequired, value); }
        }

        #endregion

        #region SelectdIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion

        #region SubSelectdIndex

        private int _subSelectedIndex;

        public int SubSelectdIndex
        {
            get { return _subSelectedIndex; }
            set { SetProperty(ref _subSelectedIndex, value); }
        }

        #endregion

        #region Converters

        private PixivThumbnailViewModel CreatePixivImage(IIllust w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        private PixivThumbnailViewModel CreatePixivNovel(INovel w) =>
            new PixivThumbnailViewModel(w, _imageStoreService, NavigationService);

        #endregion
    }
}