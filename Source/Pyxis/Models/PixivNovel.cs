using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivNovel : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly INovel _novel;

        public PixivNovel(INovel novel, IImageStoreService imageStoreService)
        {
            _novel = novel;
            _imageStoreService = imageStoreService;
            ThumbnailPath = PyxisConstants.DummyImage;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadThumbnail);

        private async Task DownloadThumbnail()
        {
            if (await _imageStoreService.ExistImageAsync(_novel.ImageUrls.Medium))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(_novel.ImageUrls.Medium);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(_novel.ImageUrls.Medium);
        }
    }
}