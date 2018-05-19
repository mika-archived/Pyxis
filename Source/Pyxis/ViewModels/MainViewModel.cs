using System.Collections.Generic;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Pyxis.Models;

namespace Pyxis.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (e.Parameter != null)
                TransitionParameter.FromQueryString<TransitionParameter>(e.Parameter as string)?.ProcessTransitionHistory(_navigationService);

            base.OnNavigatedTo(e, viewModelState);
        }
    }
}