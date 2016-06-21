using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

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