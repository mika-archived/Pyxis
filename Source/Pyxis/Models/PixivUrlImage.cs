using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivUrlImage : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly string _url;

        public PixivUrlImage(string url, IImageStoreService imageStoreService)
        {
            _url = url;
            _imageStoreService = imageStoreService;
        }

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            if (await _imageStoreService.ExistImageAsync(_url))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(_url);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(_url);
            IsProgress = false;
        }
    }
}