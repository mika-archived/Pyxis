using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class ImageStoreService : IImageStoreService
    {
        private readonly IPixivClient _client;
        private readonly Regex _origRegex = new Regex(@"^\d+_p\d+$", RegexOptions.Compiled);
        private readonly StorageFolder _temporaryFolder;
        private readonly Regex _ugoiraRegex = new Regex(@"^\d+_ugoira\d+$", RegexOptions.Compiled);
        private readonly Regex _userRegex = new Regex(@"^\d+$", RegexOptions.Compiled);

        public ImageStoreService(IPixivClient client)
        {
            _client = client;
            _temporaryFolder = ApplicationData.Current.TemporaryFolder;
            Regex.CacheSize = short.MaxValue;
        }

        public async Task<string> SaveImageAsync(string url)
        {
            try
            {
                var stream = await _client.Pximg.GetAsync(url);
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