using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivComment : ISupportIncrementalLoading
    {
        private readonly IIllust _illust;
        private readonly INovel _novel;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private string _offset;

        public ObservableCollection<IComment> Comments { get; }

        public PixivComment(IIllust illust, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _offset = "";
            Comments = new ObservableCollection<IComment>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public PixivComment(INovel novel, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _novel = novel;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            Comments = new ObservableCollection<IComment>();
            _offset = "";
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Fetch() => RunHelper.RunAsync(FetchComments);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchComments()
        {
            IComments comments;
            if (_illust != null)
                comments = await _queryCacheService.RunAsync(_pixivClient.IllustV1.CommentsAsync,
                                                             illust_id => _illust.Id,
                                                             offset => _offset);
            else
                comments = await _queryCacheService.RunAsync(_pixivClient.NovelV1.CommentsAsync,
                                                             novel_id => _novel.Id,
                                                             offset => _offset);
            comments?.CommentList.ForEach(w => Comments.Add(w));
            if (string.IsNullOrWhiteSpace(comments?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(comments.NextUrl)["offset"];
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchComments();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}