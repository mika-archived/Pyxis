using System.Linq;
using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivMangaImage : ThumbnailableBase
    {
        private readonly Illust _illust;
        private readonly IImageStoreService _imageStoreService;
        private readonly int _index;

        public PixivMangaImage(Illust illust, int index, IImageStoreService imageStoreService)
        {
            _illust = illust;
            _index = index;
            _imageStoreService = imageStoreService;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        // Support raw only
        private async Task DownloadImage()
        {
            var orig = _illust.MetaPages.ToList()[_index].ImageUrls.Original ?? _illust.MetaPages.ToList()[_index].ImageUrls.Large;
            if (await _imageStoreService.ExistImageAsync(orig))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(orig);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(orig);
            IsProgress = false;
        }
    }
}