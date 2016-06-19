using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Following
{
    public class PrivateFollowingPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public PrivateFollowingPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}