using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivRelated : ISupportIncrementalLoading
    {
        private readonly Illust _illust;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private List<int> _seedIds;
        public ObservableCollection<Illust> RelatedIllustsRoot { get; }

        public PixivRelated(Illust illust, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _seedIds = new List<int>();
            RelatedIllustsRoot = new ObservableCollection<Illust>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchRelatedItems()
        {
            var illusts = await _pixivClient.Illust.RelatedAsync(_illust.Id, "for_ios", _seedIds.ToArray());
            illusts?.Illusts.ForEach(w => RelatedIllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _seedIds = new List<int>(UrlParameter.ParseQuery(illusts.NextUrl)["seed_illust_ids[]"].Split(',').Select(int.Parse));
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchRelatedItems();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}