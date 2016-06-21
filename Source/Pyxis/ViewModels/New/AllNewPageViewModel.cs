using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.New
{
    public class AllNewPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public AllNewPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}