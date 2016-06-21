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

        public PixivImage(IIllust illust, IImageStoreService imageStoreService)
        {
            _illust = illust;
            _imageStoreService = imageStoreService;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            if (await _imageStoreService.ExistImageAsync(_illust.ImageUrls.Medium))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(_illust.ImageUrls.Medium);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(_illust.ImageUrls.Medium);
        }

        public void Save()
        {
            // TODO: 画像保存
        }
    }
}