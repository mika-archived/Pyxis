using Prism.Windows.Navigation;

namespace Pyxis.ViewModels
{
    public class WorkMainPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public WorkMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnButtonTapped()
        {
            _navigationService.Navigate("Error.LoginRequired", null);
        }
    }
}