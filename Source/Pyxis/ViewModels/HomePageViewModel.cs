using System.Collections.Generic;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        public string Username => AccountService.Account.Name;
        public List<string> Collection { get; }

        public HomePageViewModel()
        {
            Collection = new List<string> {"AAA", "AAA"};
        }
    }
}