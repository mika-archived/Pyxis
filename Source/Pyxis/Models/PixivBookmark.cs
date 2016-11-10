using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivBookmark : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private string _offset;

        public ObservableCollection<INovel> Novels { get; }

        public PixivBookmark(IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _offset = "";
            Novels = new ObservableCollection<INovel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task Fetch()
        {
            var novels = await _queryCacheService.RunAsync(_pixivClient.NovelV1.MarkersAsync, offset => _offset);
            novels?.NovelList.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await Fetch();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}