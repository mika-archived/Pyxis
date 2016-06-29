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

namespace Pyxis.Models
{
    internal class PixivBookmarkTag : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private string _offset;
        private RestrictType _restrict;
        public ObservableCollection<IBookmarkTag> BookmarkTags { get; }

        public PixivBookmarkTag(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
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
            BookmarkTags.Add(new BookmarkTag {Name = "_All"});
            BookmarkTags.Add(new BookmarkTag {Name = "Uncategorized"});
        }

        public void Query(RestrictType restrict)
        {
            Reset();
            _restrict = restrict;
            _offset = "";
#if !OFFLINE
            HasMoreItems = true;
#endif
        }

        private async Task QueryAsync()
        {
            var tags = await _pixivClient.User.BookmarkTags.IllustAsync(restrict => _restrict.ToParamString(),
                                                                        offset => _offset);
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