using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.New
{
    public class MypixivNewPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public MypixivNewPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}