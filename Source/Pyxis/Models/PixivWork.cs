using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;

namespace Pyxis.Models
{
    internal class PixivWork : ISupportIncrementalLoading
    {
        private readonly ContentType _contentType;
        private readonly string _id;
        private readonly IPixivClient _pixivClient;
        private string _offset;

        public ObservableCollection<IIllust> Illusts { get; }
        public ObservableCollection<INovel> Novels { get; }

        public PixivWork(string id, ContentType contentType, IPixivClient pixivClient)
        {
            _id = id;
            _contentType = contentType;
            _pixivClient = pixivClient;
            _offset = "";
            Illusts = new ObservableCollection<IIllust>();
            Novels = new ObservableCollection<INovel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task FetchAsync()
        {
            if (_contentType == ContentType.Illust)
                await FetchIllusts("illust");
            else if (_contentType == ContentType.Manga)
                await FetchIllusts("manga");
            else if (_contentType == ContentType.Novel)
                await FetchNovels();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchIllusts(string contentType)
        {
            var illusts = await _pixivClient.User.IllustsAsync(user_id => _id, filter => "for_ios", type => contentType,
                                                               offset => _offset);
            illusts?.IllustList.ForEach(w => Illusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchNovels()
        {
            var noves = await _pixivClient.User.NovelsAsync(user_id => _id, offset => _offset);
            noves?.NovelList.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(noves?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(noves.NextUrl)["offset"];
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchAsync();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}