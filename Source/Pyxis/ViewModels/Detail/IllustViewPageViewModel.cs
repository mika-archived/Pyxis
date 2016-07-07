using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Detail
{
    public class IllustViewPageViewModel : ThumbnailableViewModel
    {
        private readonly IImageStoreService _imageStoreService;

        public IllustViewPageViewModel(IImageStoreService imageStoreService)
        {
            _imageStoreService = imageStoreService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            var parameter = ParameterBase.ToObject<IllustDetailParameter>((string) e.Parameter);
            Thumbnailable = new PixivImage(parameter.Illust, _imageStoreService, true);
        }

        #endregion
    }
}