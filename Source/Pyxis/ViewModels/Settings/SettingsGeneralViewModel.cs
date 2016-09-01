using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Extensions;
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
            RunHelper.RunLaterUIAsync(Load, TimeSpan.FromMilliseconds(100));
        }

        // よくない
        public void ClearCache()
        {
            FileCount = "0個の項目";
            CacheSize = ((ulong) 0).GetSizeString();
            IsEnabled = false;
            Task.Run(async () =>
            {
                var temporaryFolder = ApplicationData.Current.TemporaryFolder;
                var tasks = new[]
                {
                    await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("original"),
                    await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("thumbnails"),
                    await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("users")
                }.Where(w => w != null).Select(async w => await w.DeleteAsync());
                await Task.WhenAll(tasks);
            }).ContinueWith(async w => await Load());
        }

        private async Task Load()
        {
            var temporaryFolder = ApplicationData.Current.TemporaryFolder;
            var size = 0UL;
            var count = 0U;
            var tasks = new[]
            {
                await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("original"),
                await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("thumbnails"),
                await temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("users")
            }.Where(w => w != null).Select(async w =>
            {
                var s = 0UL;
                var files = await w.GetFilesAsync();
                foreach (var file in files)
                    s += await file.GetSizeAsync();
                return new Tuple<uint, ulong>((uint) files.Count, s);
            });
            (await Task.WhenAll(tasks)).Select(w => w).ForEach(w =>
            {
                count += w.Item1;
                size += w.Item2;
            });
            CacheSize = size.GetSizeString();
            FileCount = $"{count}個";
            if (count > 0)
                IsEnabled = true;
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