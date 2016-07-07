using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Detail.Items
{
    public class PixivMangaImageViewModel : ThumbnailableViewModel
    {
        public PixivMangaImageViewModel(IIllust illust, int index, IImageStoreService imageStoreService)
        {
            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivMangaImage(illust, index, imageStoreService);
        }
    }
}