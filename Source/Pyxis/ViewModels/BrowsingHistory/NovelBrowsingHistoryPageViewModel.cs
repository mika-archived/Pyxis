using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.BrowsingHistory
{
    public class NovelBrowsingHistoryPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public NovelBrowsingHistoryPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}