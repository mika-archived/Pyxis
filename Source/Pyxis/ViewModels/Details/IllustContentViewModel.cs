using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.ViewModels.Details
{
    public class IllustContentViewModel : ViewModel
    {
        private readonly DetailsParameter _detailsParameter;
        private readonly INavigationService _navigationService;
        private readonly IObjectCacheStorage _objectCacheStorage;
        private readonly PixivClient _pixivClient;

        public IllustContentViewModel(PixivClient pixivClient, DetailsParameter parameter, INavigationService navigationService, IObjectCacheStorage objectCacheStorage)
        {
            _pixivClient = pixivClient;
            _detailsParameter = parameter;
            _navigationService = navigationService;
            _objectCacheStorage = objectCacheStorage;
        }
    }
}