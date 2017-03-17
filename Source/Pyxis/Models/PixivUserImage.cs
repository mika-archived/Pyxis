using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivUserImage : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;

        private readonly UserMini _user;

        public PixivUserImage(UserMini user, IImageStoreService imageStoreService)
        {
            _user = user;
            _imageStoreService = imageStoreService;
        }

        #region Overrides of ThumbnailableBase

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            var icon = _user.ProfileImageUrls.Medium;
            if (string.IsNullOrWhiteSpace(icon))
                return;
            if (await _imageStoreService.ExistImageAsync(icon))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(icon);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(icon);
        }

        #endregion
    }
}