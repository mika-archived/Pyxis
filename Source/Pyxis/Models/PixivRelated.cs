using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivRelated : ISupportIncrementalLoading
    {
        private readonly IIllust _illust;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private string _seedIds;
        public ObservableCollection<IIllust> RelatedIllusts { get; }

        public PixivRelated(IIllust illust, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            _seedIds = "";
            RelatedIllusts = new ObservableCollection<IIllust>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchRelatedItems()
        {
            var illusts = await _queryCacheService.RunAsync(_pixivClient.IllustV1.RelatedAsync,
                                                            illust_id => _illust.Id,
                                                            filter => "for_ios",
                                                            seed_illust_ids => _seedIds);
            illusts?.IllustList.ForEach(w => RelatedIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _seedIds = UrlParameter.ParseQuery(illusts.NextUrl)["v1_seed_illust_ids"].Replace("%2C", ",");
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