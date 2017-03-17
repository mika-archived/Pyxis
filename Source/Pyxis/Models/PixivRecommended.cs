using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

// ReSharper disable InconsistentNaming

namespace Pyxis.Models
{
    internal class PixivRecommended : ISupportIncrementalLoading
    {
        private readonly IAccountService _accountService;
        private readonly ContentType _contentType;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _maxBookmarkIdForRecommend;
        private int _minBookmarkIdForRecentIllust;
        private int _offset;

        public ObservableCollection<Illust> RecommendedImages { get; }
        public ObservableCollection<Novel> RecommendedNovels { get; }
        public ObservableCollection<UserPreview> RecommendedUsers { get; }

        public PixivRecommended(IAccountService accountService, PixivClient pixivClient, IQueryCacheService queryCacheService, ContentType contentType)
        {
            _accountService = accountService;
            _contentType = contentType;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _offset = 0;
            _maxBookmarkIdForRecommend = 0;
            _minBookmarkIdForRecentIllust = 0;
            RecommendedNovels = new ObservableCollection<Novel>();
            RecommendedImages = new ObservableCollection<Illust>();
            RecommendedUsers = new ObservableCollection<UserPreview>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task FetchRecommended()
        {
            if (_contentType == ContentType.Novel)
                await FetchNovels();
            else if (_contentType == ContentType.User)
                await FetchUsers();
            else if (_contentType == ContentType.Illust || _contentType == ContentType.Manga)
                await FetchIllustsRoot();
            else
                throw new NotSupportedException();
        }

        private async Task FetchIllustsRoot()
        {
            IllustsRoot illusts = null;
            if (_contentType == ContentType.Illust)
                illusts = await RecommendedAsync("illust");
            else if (_contentType == ContentType.Manga)
                illusts = await RecommendedAsync("manga");
            illusts?.Illusts.ForEach(w => RecommendedImages.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
            {
                HasMoreItems = false;
            }
            else
            {
                var urlParams = UrlParameter.ParseQuery(illusts.NextUrl);
                _maxBookmarkIdForRecommend = int.Parse(urlParams["max_bookmark_id_for_recommend"]);
                _minBookmarkIdForRecentIllust = int.Parse(urlParams["min_bookmark_id_for_recent_illust"]);
            }
        }

        private async Task<IllustsRoot> RecommendedAsync(string contentType)
        {
            // manga/recommended との違いがわからない。
            if (_accountService.IsLoggedIn)
                return null;
            return await _pixivClient.Illust.RecommendedAsync(maxBookmarkIdForRecommend: _maxBookmarkIdForRecommend,
                                                              minBookmarkIdForRecentIllust: _minBookmarkIdForRecentIllust);
        }

        private async Task FetchNovels()
        {
            NovelsRoot novels;
            if (_accountService.IsLoggedIn)
                novels = await _pixivClient.Novel.RecommendedAsync(offset: _offset);
            else
                novels = null;
            novels?.Novels.ForEach(w => RecommendedNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
        }

        private async Task FetchUsers()
        {
            var users = await _pixivClient.User.RecommendedAsync(offset: _offset);
            users?.UserPreviews.ForEach(w => RecommendedUsers.Add(w));
            if (string.IsNullOrWhiteSpace(users?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(users.NextUrl)["offset"]);
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchRecommended();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}