using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Microsoft.Toolkit.Uwp;

using Pyxis.Extensions;
using Pyxis.Models.Database;
using Pyxis.Services.Interfaces;

using Sagitta;

using WinRTXamlToolkit.IO.Extensions;

namespace Pyxis.Services
{
    public class FileCacheService : IFileCacheService
    {
        private readonly Regex _defaultImagesRegex = new Regex(@"\/common\/", RegexOptions.Compiled);
        private readonly Regex _novelCoveRegex = new Regex(@"\/novel-cover-master\/", RegexOptions.Compiled);
        private readonly Regex _originalRegex = new Regex(@"\/img-original\/", RegexOptions.Compiled);

        private readonly PixivClient _pixivClient;
        private readonly Regex _profileBackgroundRegex = new Regex(@"\/background\/", RegexOptions.Compiled);
        private readonly Regex _profileImageRegex = new Regex(@"\/user-profile\/", RegexOptions.Compiled);
        private readonly StorageFolder _temporaryFolder;
        private readonly Regex _thumbnailRegex = new Regex(@"\/img-master\/", RegexOptions.Compiled);
        private readonly Regex _ugoiraZipRegex = new Regex(@"\/img-zip-ugoira\/", RegexOptions.Compiled);
        private readonly Regex _workspaceRegex = new Regex(@"\/workspace\/", RegexOptions.Compiled);

        public FileCacheService(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            _temporaryFolder = ApplicationData.Current.TemporaryFolder;
            Regex.CacheSize = short.MaxValue;
        }

        public async Task<string> SaveFileAsync(string url)
        {
            if (await ExistFileAsync(url))
                return await LoadFileAsync(url);

            try
            {
                var stream = await _pixivClient.File.GetAsync(url);
                var cache = await GetDirectory(url);
                var storageFile = await cache.directory.CreateFileAsync(GetFileName(url), CreationCollisionOption.FailIfExists);
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
                    using (var db = new CacheContext())
                    {
                        await db.Caches.AddAsync(new Cache {Path = url, Size = await storageFile.GetSizeAsync(), Type = cache.type});
                        await db.SaveChangesAsync();
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
                await (await GetDirectory(url)).Item1.GetFileAsync(GetFileName(url));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> LoadFileAsync(string url)
        {
            var storageFile = await (await GetDirectory(url)).directory.GetFileAsync(GetFileName(url));
            return storageFile.Path;
        }

        public async Task CleanAsync()
        {
            var tasks = new[]
            {
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("others"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("profile-images"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("thumbnails"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("original"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("novel-covers"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("workspaces"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("background-images"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("ugoira-zip"),
                await _temporaryFolder.GetFolderWhenNotFoundReturnNullAsync("common")
            }.Where(w => w != null).Select(async w => await w.DeleteAsync());
            await Task.WhenAll(tasks);
        }

        public async Task SaveFileToLocalAsync(string url, string dist)
        {
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(dist);
            var name = GetFileName(url);
            if (await storageFolder.FileExistsAsync(name))
                return;
            var storageFile = await storageFolder.CreateFileAsync(name);
            var sourceFile = await LoadFileAsync(url);
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

        private async Task<(StorageFolder directory, CacheType type)> GetDirectory(string url)
        {
            var tuple = ("others", CacheType.Others);
            // Profile Image -> /user-profile/img/DATETIME/{user_id}_{hash}_SIZE.{ext}
            if (_profileImageRegex.IsMatch(url))
                tuple = ("profile-images", CacheType.ProfileImage);
            // Thumbnail -> /c/SIZE/img-master/img/DATETIME/{illust_id}_p{page}_SIZE.{ext}
            else if (_thumbnailRegex.IsMatch(url))
                tuple = ("thumbnails", CacheType.MasterImage);
            // Original -> /img-original/img/DATETIME/{illust_id}_p{page}.{ext}
            else if (_originalRegex.IsMatch(url))
                tuple = ("original", CacheType.Original);
            // Novel Cover -> /c/SIZE/novel-cover-master/img/DATETIME/{novel_id}_{hash}_master_SIZE.{ext}
            else if (_novelCoveRegex.IsMatch(url))
                tuple = ("novel-covers", CacheType.NovelCover);
            // Workspace -> /workspace/img/DAETIME/{user_id}_{hash}.{ext}
            else if (_workspaceRegex.IsMatch(url))
                tuple = ("workspaces", CacheType.Workspace);
            // Profile Background -> /c/SIZE/background/img/DATETIME/{user_id}_{hash}.{ext}
            else if (_profileBackgroundRegex.IsMatch(url))
                tuple = ("background-images", CacheType.ProfileBackground);
            // Ugoira Zip -> /img-zip-ugoira/img/DATETIME/{illust_id}_ugoiraSIZE.zip
            else if (_ugoiraZipRegex.IsMatch(url))
                tuple = ("ugoira-zip", CacheType.UgoiraZip);
            // Default Images -> /common/***
            else if (_defaultImagesRegex.IsMatch(url))
                tuple = ("common", CacheType.DefaultImages);

            return (await _temporaryFolder.CreateFolderAsync(tuple.Item1, CreationCollisionOption.OpenIfExists), tuple.Item2);
        }

        private static string GetFileName(string url)
        {
            var name = Path.GetFileName(url);
            if (Path.GetInvalidFileNameChars().Any(w => name.Contains(w)))
                name = Path.GetInvalidFileNameChars().Aggregate(name, (current, w) => current.Replace(w, '_'));
            return name;
        }
    }
}