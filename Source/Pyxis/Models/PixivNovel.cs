using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivNovel : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly Novel _novel;

        public PixivNovel(Novel novel, IImageStoreService imageStoreService)
        {
            _novel = novel;
            _imageStoreService = imageStoreService;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadThumbnail);

        private async Task DownloadThumbnail()
        {
            if (await _imageStoreService.ExistImageAsync(_novel.ImageUrls.SquareMedium))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(_novel.ImageUrls.SquareMedium);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(_novel.ImageUrls.SquareMedium);
        }
    }
}