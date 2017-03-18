using System.Collections.Generic;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        private readonly IAccountService _accountService;

        public string Username => _accountService.Account.Name;
        public List<string> Collection { get; }

        public HomePageViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            Collection = new List<string> {"AAA", "AAA"};
        }
    }
}