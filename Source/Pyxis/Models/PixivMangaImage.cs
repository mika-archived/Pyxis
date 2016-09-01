using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivMangaImage : ThumbnailableBase
    {
        private readonly IIllust _illust;
        private readonly IImageStoreService _imageStoreService;
        private readonly int _index;

        public PixivMangaImage(IIllust illust, int index, IImageStoreService imageStoreService)
        {
            _illust = illust;
            _index = index;
            _imageStoreService = imageStoreService;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        // Support raw only
        private async Task DownloadImage()
        {
            var orig = _illust.MetaPages[_index].ImageUrls.Original ?? _illust.MetaPages[_index].ImageUrls.Large;
            if (await _imageStoreService.ExistImageAsync(orig))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(orig);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(orig);
            IsProgress = false;
        }
    }
}