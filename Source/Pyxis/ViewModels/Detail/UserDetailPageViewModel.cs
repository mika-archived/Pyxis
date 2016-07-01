using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailPageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;

        public UserDetailPageViewModel(IAccountService accountService, INavigationService navigationService,
                                       IPixivClient pixivClient)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
        }

        private void Initialie(UserDetailParameter parameter)
        {
            var id = parameter.User?.Id ?? _accountService.LoggedInAccount.Id;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<UserDetailParameter>((string) e?.Parameter);
            Initialie(parameter);
        }

        #endregion
    }
}