using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Favorite
{
    public class NovelFavoritePageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public NovelFavoritePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}