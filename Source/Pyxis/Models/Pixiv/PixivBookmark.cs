using System.Collections.Generic;
using System.Threading.Tasks;

using Pyxis.Extensions;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    internal class PixivBookmark : PixivModel
    {
        private readonly ISessionObjectStorageService _objectStorage;

        public PixivBookmark(PixivClient pixivClient, ISessionObjectStorageService objectStorage) : base(pixivClient)
        {
            _objectStorage = objectStorage;
            if (!_objectStorage.ExistValue("Illust-FavIds"))
                _objectStorage.AddValue("Illust-FavIds", new List<int>());
            if (!_objectStorage.ExistValue("Novel-FavIds"))
                _objectStorage.AddValue("Novel-FavIds", new List<int>());
        }

        public async Task BookmarkAsync(Post post)
        {
            var ids = _objectStorage.GetValue<List<int>>($"{post.GetIdentifier()}-FavIds");
            if (ids.Contains(post.Id))
                return;

            if (post.GetIdentifier() == "Illust")
                await PixivClient.Illust.Bookmark.AddAsync(post.Id);
            else
                await PixivClient.Novel.Bookmark.AddAsync(post.Id);
            ids.Add(post.Id);
            _objectStorage.AddValue($"{post.GetIdentifier()}-FavIds", ids);
        }

        public async Task UnBookmarkAsync(Post post)
        {
            var ids = _objectStorage.GetValue<List<int>>($"{post.GetIdentifier()}-FavIds");
            if (ids.Contains(post.Id))
                return;

            if (post.GetIdentifier() == "Illust")
                await PixivClient.Illust.Bookmark.AddAsync(post.Id);
            else
                await PixivClient.Novel.Bookmark.AddAsync(post.Id);
            ids.Remove(post.Id);
            _objectStorage.AddValue($"{post.GetIdentifier()}-FavIds", ids);
        }

        public bool IsBookmarked(Post post)
        {
            var ids = _objectStorage.GetValue<List<int>>($"{post.GetIdentifier()}-FavIds");
            if (ids.Contains(post.Id))
                return true;
            if (!post.IsBookmarked)
                return post.IsBookmarked;
            ids.Add(post.Id);
            _objectStorage.AddValue($"{post.GetIdentifier()}-FavIds", ids);
            return post.IsBookmarked;
        }
    }
}