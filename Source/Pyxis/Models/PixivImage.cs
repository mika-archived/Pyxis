using System.Linq;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivImage : ThumbnailableBase
    {
        private readonly IIllust _illust;
        private readonly IImageStoreService _imageStoreService;
        private readonly bool _isRaw;

        public PixivImage(IIllust illust, IImageStoreService imageStoreService, bool isRaw = false)
        {
            _illust = illust;
            _imageStoreService = imageStoreService;
            _isRaw = isRaw;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            if (!_isRaw)
            {
                if (await _imageStoreService.ExistImageAsync(_illust.ImageUrls.SquareMedium))
                    ThumbnailPath = await _imageStoreService.LoadImageAsync(_illust.ImageUrls.SquareMedium);
                else
                    ThumbnailPath = await _imageStoreService.SaveImageAsync(_illust.ImageUrls.SquareMedium);
            }
            else
            {
                var orig = _illust.MetaPages.FirstOrDefault()?.ImageUrls.Original ??
                           _illust.MetaSinglePage.Original ?? _illust.ImageUrls.Large;
                if (await _imageStoreService.ExistImageAsync(orig))
                    ThumbnailPath = await _imageStoreService.LoadImageAsync(orig);
                else
                    ThumbnailPath = await _imageStoreService.SaveImageAsync(orig);
            }
        }
    }
}