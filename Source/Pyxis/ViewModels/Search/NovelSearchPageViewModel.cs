using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Search
{
    public class NovelSearchPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public NovelSearchPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}