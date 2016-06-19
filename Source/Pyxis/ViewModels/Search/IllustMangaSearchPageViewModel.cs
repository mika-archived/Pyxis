using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Search
{
    public class IllustMangaSearchPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public IllustMangaSearchPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}