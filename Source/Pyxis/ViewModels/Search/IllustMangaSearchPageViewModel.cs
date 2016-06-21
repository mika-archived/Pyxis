using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

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