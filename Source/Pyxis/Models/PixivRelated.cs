using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;

namespace Pyxis.Models
{
    internal class PixivRelated : ISupportIncrementalLoading
    {
        private readonly IIllust _illust;
        private readonly IPixivClient _pixivClient;
        private string _seedIds;
        public ObservableCollection<IIllust> RelatedIllusts { get; }

        public PixivRelated(IIllust illust, IPixivClient pixivClient)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            _seedIds = "a";
            RelatedIllusts = new ObservableCollection<IIllust>();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchRelatedItems()
        {
            IIllusts illusts;
            if (_seedIds == "a")
                illusts = await _pixivClient.IllustV1.RelatedAsync(illust_id => _illust.Id,
                                                                   filter => "for_ios");
            else
                illusts = await _pixivClient.IllustV1.RelatedAsync(illust_id => _illust.Id,
                                                                   filter => "for_ios",
                                                                   seed_illust_ids => _seedIds);
            illusts?.IllustList.ForEach(w => RelatedIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
            {
                _seedIds = null;
                return;
            }
            var str = illusts.NextUrl;
            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            var seedIllustIds = str.Substring(str.IndexOf("seed_illust_ids=") + "seed_illust_ids=".Length);
            _seedIds = seedIllustIds.Replace("%2C", ",");
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

        public bool HasMoreItems => !string.IsNullOrWhiteSpace(_seedIds);

        #endregion
    }
}