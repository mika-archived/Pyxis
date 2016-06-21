using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Error
{
    public class PremiumRequiredPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public PremiumRequiredPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("http://www.pixiv.net/premium.php"));

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            ClearNavigationHistory();
        }

        #endregion

        private void ClearNavigationHistory()
        {
            _navigationService.RemoveLastPage();
        }
    }
}