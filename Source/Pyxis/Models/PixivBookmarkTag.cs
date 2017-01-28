using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivBookmarkTag : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private string _offset;
        private RestrictType _restrict;
        private SearchType _searchType;

        public ObservableCollection<IBookmarkTag> BookmarkTags { get; }

        public PixivBookmarkTag(IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            BookmarkTags = new ObservableCollection<IBookmarkTag>();
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
            _offset = "";
#if !OFFLINE
            HasMoreItems = true;
#endif
        }

        private async Task QueryAsync()
        {
            IBookmarkTags tags;
            if (_searchType == SearchType.IllustsAndManga)
                tags = await _queryCacheService.RunAsync(_pixivClient.UserV1.BookmarkTags.IllustAsync,
                                                         restrict => _restrict.ToParamString(),
                                                         offset => _offset);
            else if (_searchType == SearchType.Novels)
                tags = await _queryCacheService.RunAsync(_pixivClient.UserV1.BookmarkTags.NovelAsync,
                                                         restrict => _restrict.ToParamString(),
                                                         offset => _offset);
            else
                throw new NotSupportedException();
            tags?.BookmarkTagList.ForEach(w => BookmarkTags.Add(w));
            if (string.IsNullOrWhiteSpace(tags?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(tags.NextUrl)["offset"];
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