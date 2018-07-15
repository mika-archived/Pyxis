using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.ViewModels
{
    public class DetailsViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public DetailsViewModel(PixivClient pixivClient, INavigationService navigationService, IObjectCacheStorage objectCacheStorage)
        {
            _navigationService = navigationService;
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (e.Parameter != null)
            {
                var parameter = TransitionParameter.FromQueryString<DetailsParameter>(e.Parameter as string);
                parameter?.ProcessTransitionHistory(_navigationService);
            }

            base.OnNavigatedTo(e, viewModelState);
        }
    }
}