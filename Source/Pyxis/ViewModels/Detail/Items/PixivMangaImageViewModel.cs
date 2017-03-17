using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Detail.Items
{
    public class PixivMangaImageViewModel : ThumbnailableViewModel
    {
        public PixivMangaImageViewModel(Illust illust, int index, IImageStoreService imageStoreService)
        {
            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivMangaImage(illust, index, imageStoreService);
        }
    }
}