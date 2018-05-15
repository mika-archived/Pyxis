using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Microsoft.Toolkit.Uwp.Helpers;

using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.Services
{
    internal class PixivCacheStorage : IFileCacheStorage
    {
        private readonly PixivClient _pixivClient;
        private readonly StorageFolder _temporaryDirectory;
        private StorageFolder _cacheDirectory;

        public PixivCacheStorage(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            _temporaryDirectory = ApplicationData.Current.TemporaryFolder;
        }

        public async Task<string> SaveFileAsync(string url)
        {
            if (await ExistFileAsync(url))
                return await LoadFileAsync(url);

            try
            {
                var stream = await _pixivClient.File.GetAsync(url);
                var directory = await GetDirectory();
                var storageFile = await directory.CreateFileAsync(Path.GetFileName(url), CreationCollisionOption.FailIfExists);
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return await LoadFileAsync(url);
        }

        public async Task<bool> ExistFileAsync(string url)
        {
            try
            {
                await (await GetDirectory()).GetFileAsync(Path.GetFileName(url));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> LoadFileAsync(string url)
        {
            var storageFile = await (await GetDirectory()).GetFileAsync(Path.GetFileName(url));
            return storageFile.Path;
        }

        public async Task ClearAsync()
        {
            await (await GetDirectory()).DeleteAsync();
        }

        public async Task SaveFileToLocalAsync(string url, string local)
        {
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(local);
            var name = Path.GetFileName(url);
            if (await storageFolder.FileExistsAsync(name))
                return;
            var storageFile = await storageFolder.CreateFileAsync(name);
            var sourceFile = await SaveFileAsync(url);
            using (var stream = new FileStream(sourceFile, FileMode.Open))
            {
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
        }

        private async Task<StorageFolder> GetDirectory()
        {
            return _cacheDirectory ?? (_cacheDirectory = await _temporaryDirectory.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists));
        }
    }
}