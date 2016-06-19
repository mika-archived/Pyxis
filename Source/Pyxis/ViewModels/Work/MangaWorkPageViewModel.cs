using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Work
{
    public class MangaWorkPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public MangaWorkPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}