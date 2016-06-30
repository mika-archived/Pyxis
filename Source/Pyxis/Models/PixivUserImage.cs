using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivUserImage : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;

        private readonly IUserBase _user;

        public PixivUserImage(IUserBase user, IImageStoreService imageStoreService)
        {
            _user = user;
            _imageStoreService = imageStoreService;
        }

        #region Overrides of ThumbnailableBase

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            var icon = _user.ProfileImageUrls.Size50 ?? _user.ProfileImageUrls.Medium;
            if (await _imageStoreService.ExistImageAsync(icon))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(icon);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(icon);
        }

        #endregion
    }
}