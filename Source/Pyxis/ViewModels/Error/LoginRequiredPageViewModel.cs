using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Error
{
    public class LoginRequiredPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private string _fromPageToken;

        public LoginRequiredPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public void OnLoginButtonTapped() => _navigationService.Navigate("Account.Login", _fromPageToken);

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _fromPageToken = e.Parameter as string;
            ClearNavigationHistory();
        }

        #endregion

        private void ClearNavigationHistory()
        {
            _navigationService.RemoveLastPage();
        }
    }
}