using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Work
{
    public class NovelWorkPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public NovelWorkPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}