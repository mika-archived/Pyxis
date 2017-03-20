using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class UserPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public UserPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}