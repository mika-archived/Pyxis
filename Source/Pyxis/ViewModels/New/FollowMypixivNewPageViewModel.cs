using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.New
{
    public class FollowMypixivNewPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        public INavigationService NavigationService { get; }

        public FollowMypixivNewPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            NavigationService = navigationService;
            IsLoggedInRequired = !_accountService.IsLoggedIn;
        }

        private void Initialize(NewParameter parameter)
        {
            SelectedIndex = (int) parameter.FollowType;
            SubSelectdIndex = (int) parameter.ContentType;
            IsLoggedInRequired = !_accountService.IsLoggedIn;
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
    }
}