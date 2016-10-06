using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;
using Pyxis.Models.Cache;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using WinRTXamlToolkit.IO.Extensions;

namespace Pyxis.ViewModels.Settings
{
    public class SettingsMaintenanceViewModel : ResourceViewModel
    {
        private readonly IImageStoreService _imageStoreService;

        public SettingsMaintenanceViewModel(IImageStoreService imageStoreService)
        {
            _imageStoreService = imageStoreService;
            CacheSize = Resources.GetString("Calculating/Text");
            FileCount = Resources.GetString("Calculating/Text");
            RunHelper.RunLaterUI(CalcCacheSize, TimeSpan.FromMilliseconds(100));
        }

        private void CalcCacheSize()
        {
            using (var db = new CacheContext())
            {
                var count = db.CacheFiles.Count();
                FileCount = string.Format(Resources.GetString("Items/Text"), count);
                CacheSize = ((ulong) db.CacheFiles.Select(w => w.Size).Sum()).GetSizeString();
                IsEnabled = count > 0;
            }
        }

        public void ClearCache()
        {
            FileCount = Resources.GetString("ZeroItems/Text");
            CacheSize = ((ulong) 0).GetSizeString();
            IsEnabled = false;
            Task.Run(async () =>
            {
                await _imageStoreService.ClearImagesAsync();
                using (var db = new CacheContext())
                {
                    // ReSharper disable once AccessToDisposedClosure
                    db.CacheFiles.ForEach(w => db.CacheFiles.Remove(w));
                    db.SaveChanges();
                }
            }).ContinueWith(w => CalcCacheSize());
        }

        #region FileCount

        private string _fileCount;

        public string FileCount
        {
            get { return _fileCount; }
            set { SetProperty(ref _fileCount, value); }
        }

        #endregion

        #region CacheSize

        private string _cacheSize;

        public string CacheSize
        {
            get { return _cacheSize; }
            set { SetProperty(ref _cacheSize, value); }
        }

        #endregion

        #region IsEnabled

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        #endregion
    }
}