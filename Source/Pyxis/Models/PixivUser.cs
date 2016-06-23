using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Models.Base;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivUser : ThumbnailableBase
    {
        private readonly IImageStoreService _imageStoreService;

        private readonly IUserBase _user;

        public PixivUser(IUserBase user, IImageStoreService imageStoreService)
        {
            _user = user;
            _imageStoreService = imageStoreService;
        }

        #region Overrides of ThumbnailableBase

        public override void ShowThumbnail() => RunHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            if (await _imageStoreService.ExistImageAsync(_user.ProfileImageUrls.Size50))
                ThumbnailPath = await _imageStoreService.LoadImageAsync(_user.ProfileImageUrls.Size50);
            else
                ThumbnailPath = await _imageStoreService.SaveImageAsync(_user.ProfileImageUrls.Size50);
        }

        #endregion
    }
}