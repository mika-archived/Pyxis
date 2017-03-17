using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivBookmark : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _offset;

        public ObservableCollection<Novel> Novels { get; }

        public PixivBookmark(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _offset = 0;
            Novels = new ObservableCollection<Novel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task Fetch()
        {
            var novels = await _pixivClient.Novel.MarkersAsync(_offset);
            novels?.MarkedNovels.ForEach(w => Novels.Add(w.Novel));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
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