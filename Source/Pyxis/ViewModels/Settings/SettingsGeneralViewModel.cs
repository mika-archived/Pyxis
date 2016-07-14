using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;
using Pyxis.ViewModels.Base;

using WinRTXamlToolkit.IO.Extensions;

namespace Pyxis.ViewModels.Settings
{
    public class SettingsGeneralViewModel : ViewModel
    {
        public SettingsGeneralViewModel()
        {
            CacheSize = "計算中...";
            FileCount = "計算中...";
            RunHelper.RunLaterAsync(Load, TimeSpan.FromMilliseconds(100));
        }

        // よくない
        public async Task ClearCache()
        {
            CacheSize = "0 Byte";
            IsEnabled = false;
            var temporaryFolder = ApplicationData.Current.TemporaryFolder;
            var files = await temporaryFolder.GetFilesAsync();
            files.AsParallel().ForEach(async w => await w.DeleteAsync());
        }

        private async Task Load()
        {
            var temporaryFolder = ApplicationData.Current.TemporaryFolder;
            var files = await temporaryFolder.GetFilesAsync();
            FileCount = $"{files.Count}個の項目";
            if (files.Count == 0)
            {
                CacheSize = "0 Byte";
                IsEnabled = false;
                return;
            }
            IsEnabled = true;
            ulong size = 0;
            foreach (var storageFile in files)
                size += await storageFile.GetSizeAsync();

            if (size < 1024)
                CacheSize = $"{size} Bytes";
            else if (size < 1024 * 1024)
                CacheSize = $"{size / 1024} KB";
            else if (size < 1024 * 1024 * 1024)
                CacheSize = $"{size / (1024 * 1024)} MB";
            else
                CacheSize = $"{size / Math.Pow(1024, 3)} GB";
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