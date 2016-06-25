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

namespace Pyxis.Models
{
    internal class PixivComment : ISupportIncrementalLoading
    {
        private readonly IIllust _illust;
        private readonly IPixivClient _pixivClient;

        public ObservableCollection<IComment> Comments { get; }

        public PixivComment(IIllust illust, IPixivClient pixivClient)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            HasMoreItems = true;
            Comments = new ObservableCollection<IComment>();
        }

        public void Fetch() => RunHelper.RunAsync(FetchComments);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchComments()
        {
            var comments = await _pixivClient.IllustV1.CommentsAsync(illust_id => _illust.Id, offset => Comments.Count);
            comments?.CommentList.ForEach(w => Comments.Add(w));
            if (string.IsNullOrWhiteSpace(comments?.NextUrl))
                HasMoreItems = false;
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