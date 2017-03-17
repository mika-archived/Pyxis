using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivComment : ISupportIncrementalLoading
    {
        private readonly Illust _illust;
        private readonly Novel _novel;
        private readonly PixivClient _pixivClient;
        private int _offset;

        public ObservableCollection<Comment> Comments { get; }

        public PixivComment(Illust illust, PixivClient pixivClient)
        {
            _illust = illust;
            _pixivClient = pixivClient;
            _offset = 0;
            Comments = new ObservableCollection<Comment>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public PixivComment(Novel novel, PixivClient pixivClient)
        {
            _novel = novel;
            _pixivClient = pixivClient;
            Comments = new ObservableCollection<Comment>();
            _offset = 0;
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
            CommentsRoot comments;
            if (_illust != null)
                comments = await _pixivClient.Illust.CommentAsync(_illust.Id, offset: _offset);
            else
                comments = await _pixivClient.Novel.CommentsAsync(_novel.Id, _offset);
            comments?.Comments.ForEach(w => Comments.Add(w));
            if (string.IsNullOrWhiteSpace(comments?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(comments.NextUrl)["offset"]);
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