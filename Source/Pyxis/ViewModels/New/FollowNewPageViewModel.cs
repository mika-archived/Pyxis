using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.New
{
    public class FollowNewPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public FollowNewPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}