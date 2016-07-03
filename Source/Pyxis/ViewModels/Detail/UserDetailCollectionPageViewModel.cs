using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailCollectionPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public UserDetailCollectionPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}