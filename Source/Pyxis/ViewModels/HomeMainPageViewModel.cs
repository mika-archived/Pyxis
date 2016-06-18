using Prism.Windows.Navigation;

namespace Pyxis.ViewModels
{
    public class HomeMainPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public HomeMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}