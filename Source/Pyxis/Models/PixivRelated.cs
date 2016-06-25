using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;

namespace Pyxis.Models
{
    internal class PixivRelated : ISupportIncrementalLoading
    {
        private readonly IIllust _illust;
        private readonly IPixivClient _pixivClient;
        public ObservableCollection<IIllust> RelatedIllusts { get; }

        public PixivRelated(IIllust illust, IPixivClient pixivClient)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            RelatedIllusts = new ObservableCollection<IIllust>();
        }

        private async Task FetchRelatedItems()
        {

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

        public bool HasMoreItems => false;

        #endregion
    }
}