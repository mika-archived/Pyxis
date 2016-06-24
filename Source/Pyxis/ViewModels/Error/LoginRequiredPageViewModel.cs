using System;
using System.Collections.Generic;

using Windows.System;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Error
{
    public class LoginRequiredPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private RedirectParameter _parameter;

        public LoginRequiredPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void OnRegisterButtonTapped()
            => await Launcher.LaunchUriAsync(new Uri("https://accounts.pixiv.net/signup"));

        public void OnLoginButtonTapped() => _navigationService.Navigate("Account.Login", _parameter.ToJson());

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _parameter = ParameterBase.ToObject<RedirectParameter>(e.Parameter?.ToString());
            _navigationService.RemoveLastPage();
        }

        #endregion
    }
}