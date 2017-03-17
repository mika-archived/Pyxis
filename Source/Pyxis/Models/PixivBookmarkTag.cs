using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivBookmarkTag : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _offset;
        private RestrictType _restrict;
        private SearchType _searchType;

        public ObservableCollection<BookmarkTag> BookmarkTags { get; }

        public PixivBookmarkTag(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            BookmarkTags = new ObservableCollection<BookmarkTag>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private void Reset()
        {
            BookmarkTags.Clear();
            BookmarkTags.Add(new BookmarkTag {Name = "すべて"});
            BookmarkTags.Add(new BookmarkTag {Name = "未分類"});
        }

        public void Query(SearchType searchType, RestrictType restrict)
        {
            Reset();
            _searchType = searchType;
            _restrict = restrict;
            _offset = 0;
#if !OFFLINE
            HasMoreItems = true;
#endif
        }

        private async Task QueryAsync()
        {
            BookmarkTags tags;
            if (_searchType == SearchType.IllustsAndManga)
                tags = await _pixivClient.User.BookmarkTags.IllustAsync(Restrict.Public, _offset);
            else if (_searchType == SearchType.Novels)
                tags = await _pixivClient.User.BookmarkTags.NovelAsync(Restrict.Public, _offset);
            else
                throw new NotSupportedException();
            tags?.Tags.ForEach(w => BookmarkTags.Add(w));
            if (string.IsNullOrWhiteSpace(tags?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(tags.NextUrl)["offset"]);
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await QueryAsync();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}