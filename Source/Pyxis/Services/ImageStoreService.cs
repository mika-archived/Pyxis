using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Pyxis.Services.Interfaces;

using Sagitta;

using WinRTXamlToolkit.IO.Extensions;

namespace Pyxis.Services
{
    internal class ImageStoreService : IImageStoreService
    {
        private readonly Regex _backgroundRegex = new Regex(@"^\d+_[a-z0-9]{32}$", RegexOptions.Compiled);
        private readonly PixivClient _client;
        private readonly Regex _origRegex = new Regex(@"^\d+_p\d+$", RegexOptions.Compiled);
        private readonly StorageFolder _temporaryFolder;
        private readonly Regex _ugoiraRegex = new Regex(@"^\d+_ugoira\d+$", RegexOptions.Compiled);
        private readonly Regex _userRegex = new Regex(@"^\d+$", RegexOptions.Compiled);

        public ImageStoreService(PixivClient client)
        {
            _client = client;
            _temporaryFolder = ApplicationData.Current.TemporaryFolder;
            Regex.CacheSize = short.MaxValue;
        }

        public async Task<string> SaveImageAsync(string url)
        {
            try
            {
                var stream = await _client.Image.GetAsync(url);
                var storageFile = await (await GetDirectory(url))
                    .CreateFileAsync(GetFileId(url), CreationCollisionOption.FailIfExists);
                using (var transaction = await storageFile.OpenTransactedWriteAsync())
                {
                    using (var writer = new DataWriter(transaction.Stream))
                    {
                        int b;
                        while ((b = stream.ReadByte()) != -1)
                            writer.WriteByte((byte) b);
                        transaction.Stream.Size = await writer.StoreAsync();
                        await transaction.CommitAsync();
                    }
                }
                return await LoadImageAsync(url);
            }
            catch
            {
                // アクセス違反
                return await LoadImageAsync(url);
            }
        }

        public async Task<bool> ExistImageAsync(string url)
        {
            try
            {
                await (await GetDirectory(url)).GetFileAsync(GetFileId(url));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> LoadImageAsync(string url)
        {
            var storageFile = await (await GetDirectory(url)).GetFileAsync(GetFileId(url));
            return storageFile.Path;
        }

        public async Task SaveToLocalFolderAsync(string url)
        {
            var pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            StorageFolder picturesFolder = null;
            foreach (var folder in pictures.Folders)
                if (!folder.Path.Contains("OneDrive"))
                    picturesFolder = folder;
            if (picturesFolder == null)
                throw new NotSupportedException();
            await picturesFolder.EnsureFolderExistsAsync(PyxisConstants.DownloadFoilderName);
            var downloadFolder = await picturesFolder.GetFolderAsync(PyxisConstants.DownloadFoilderName);
            if (await IsExistsFile(downloadFolder, GetFileId(url)))
                return;
            var storageFile = await downloadFolder.CreateFileAsync(GetFileId(url));
            var imagePath = await LoadImageAsync(url);
            var stream = new FileStream(imagePath, FileMode.Open);
            using (var transaction = await storageFile.OpenTransactedWriteAsync())
            {
                using (var writer = new DataWriter(transaction.Stream))
                {
                    int b;
                    while ((b = stream.ReadByte()) != -1)
                        writer.WriteByte((byte) b);
                    transaction.Stream.Size = await writer.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        private async Task<bool> IsExistsFile(IStorageFolder folder, string filename)
        {
            try
            {
                await folder.GetFileAsync(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<StorageFolder> GetDirectory(string url)
        {
            var value = Path.GetFileNameWithoutExtension(GetFileId(url));
            if (value.EndsWith("square1200"))
                return await _temporaryFolder.CreateFolderAsync("thumbnails", CreationCollisionOption.OpenIfExists);
            if (_origRegex.IsMatch(value))
                return await _temporaryFolder.CreateFolderAsync("original", CreationCollisionOption.OpenIfExists);
            if (_ugoiraRegex.IsMatch(value))
                return await _temporaryFolder.CreateFolderAsync("ugoira", CreationCollisionOption.OpenIfExists);
            if (value.EndsWith("170") || value.EndsWith("_s") || _userRegex.IsMatch(value))
                return await _temporaryFolder.CreateFolderAsync("users", CreationCollisionOption.OpenIfExists);
            if (_backgroundRegex.IsMatch(value))
                return await _temporaryFolder.CreateFolderAsync("background", CreationCollisionOption.OpenIfExists);
            return _temporaryFolder;
        }

        private string GetFileId(string url)
        {
            var value = Path.GetFileName(url);
            if (Path.GetInvalidFileNameChars().Any(w => value.Contains(w)))
                value = Path.GetInvalidFileNameChars().Aggregate(value, (current, c) => current.Replace(c, '_'));
            return value;
        }
    }
}