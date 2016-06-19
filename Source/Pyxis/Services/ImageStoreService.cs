using System;
using System.IO;
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
        private readonly StorageFolder _temporaryFolder;

        public ImageStoreService(IPixivClient client)
        {
            _client = client;
            _temporaryFolder = ApplicationData.Current.TemporaryFolder;
        }

        public async Task<string> SaveImageAsync(string url)
        {
            var stream = await _client.Pximg.GetAsync(url);
            var storageFile =
                await _temporaryFolder.CreateFileAsync(GetFileId(url), CreationCollisionOption.ReplaceExisting);
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

        public async Task<bool> ExistImageAsync(string url)
        {
            try
            {
                await _temporaryFolder.GetFileAsync(GetFileId(url));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> LoadImageAsync(string url)
        {
            var storageFile = await _temporaryFolder.GetFileAsync(GetFileId(url));
            return storageFile.Path;
        }

        private string GetFileId(string url)
        {
            var value = Path.GetFileName(url);
            return value;
        }
    }
}