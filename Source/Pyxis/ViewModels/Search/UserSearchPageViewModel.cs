using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Search
{
    public class UserSearchPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public UserSearchPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}