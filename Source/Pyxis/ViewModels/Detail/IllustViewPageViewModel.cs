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
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;

        public IllustViewPageViewModel(ICategoryService categoryService, IImageStoreService imageStoreService)
        {
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            var parameter = ParameterBase.ToObject<IllustDetailParameter>((string) e.Parameter);
            _categoryService.UpdateCategory();
            Thumbnailable = new PixivImage(parameter.Illust, _imageStoreService, true);
        }

        #endregion
    }
}