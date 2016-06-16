using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivImage : BindableBase
    {
        private readonly IIllust _illust;
        private readonly IImageStoreService _imageStoreService;

        public PixivImage(IIllust illust, IImageStoreService imageStoreService)
        {
            _illust = illust;
            _imageStoreService = imageStoreService;
            ImagePath = "https://placehold.jp/1x1.png";
        }

        // 表示する際に。
        public void Show() => AsyncHelper.RunAsync(DownloadImage);

        private async Task DownloadImage()
        {
            if (await _imageStoreService.ExistImageAsync(_illust.ImageUrls.Medium))
                ImagePath = await _imageStoreService.LoadImageAsync(_illust.ImageUrls.Medium);
            else
                ImagePath = await _imageStoreService.SaveImageAsync(_illust.ImageUrls.Medium);
        }

        public void Save()
        {
            // TODO: 画像保存
        }

        #region ImagePath

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        #endregion
    }
}