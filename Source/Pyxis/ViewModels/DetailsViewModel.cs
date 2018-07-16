using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Details;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels
{
    public class DetailsViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IObjectCacheStorage _objectCacheStorage;
        private readonly PixivClient _pixivClient;

        public DetailsViewModel(PixivClient pixivClient, INavigationService navigationService, IObjectCacheStorage objectCacheStorage)
        {
            _pixivClient = pixivClient;
            _navigationService = navigationService;
            _objectCacheStorage = objectCacheStorage;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (e.Parameter != null)
            {
                var parameter = TransitionParameter.FromQueryString<DetailsParameter>(e.Parameter as string);
                parameter.ProcessTransitionHistory(_navigationService);

                if (parameter.Type == typeof(Illust))
                    ContentViewModel = new IllustContentViewModel(_pixivClient, parameter, _navigationService, _objectCacheStorage);
            }

            base.OnNavigatedTo(e, viewModelState);
        }

        #region ContentViewModel

        private ViewModel _contentViewModel;

        public ViewModel ContentViewModel
        {
            get => _contentViewModel;
            set => SetProperty(ref _contentViewModel, value);
        }

        #endregion
    }
}