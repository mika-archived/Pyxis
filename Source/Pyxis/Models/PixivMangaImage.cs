using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivMangaImage
    {
        private readonly IIllust _illust;
        private readonly IImageStoreService _imageStoreService;

        public ObservableCollection<string> ThumbnailPaths { get; }

        public PixivMangaImage(IIllust illust, IImageStoreService imageStoreService)
        {
            _illust = illust;
            _imageStoreService = imageStoreService;
            ThumbnailPaths = new ObservableCollection<string>();
        }

        public void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        // Support raw only
        private async Task DownloadImage()
        {
            foreach (var imageUrls in _illust.MetaPages)
            {
                var orig = imageUrls.ImageUrls.Original ?? imageUrls.ImageUrls.Large;
                if (await _imageStoreService.ExistImageAsync(orig))
                    ThumbnailPaths.Add(await _imageStoreService.LoadImageAsync(orig));
                else
                    ThumbnailPaths.Add(await _imageStoreService.SaveImageAsync(orig));
            }
        }
    }
}