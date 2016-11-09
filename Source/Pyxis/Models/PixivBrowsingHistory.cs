using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivBrowsingHistory : ISupportIncrementalLoading
    {
        private readonly ContentType2 _contentType;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private string _offset;

        public ObservableCollection<IIllust> Illusts { get; }
        public ObservableCollection<INovel> Novels { get; }

        public PixivBrowsingHistory(IPixivClient pixivClient, ContentType2 contentType, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _contentType = contentType;
            _queryCacheService = queryCacheService;
            _offset = "";
            Illusts = new ObservableCollection<IIllust>();
            Novels = new ObservableCollection<INovel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        #region Implementation of ISupportIncrementalLoading

        private async Task Fetch()
        {
            if (_contentType == ContentType2.IllustAndManga)
                await FetchIllusts();
            else
                await FetchNovels();
            ;
        }

        private async Task FetchIllusts()
        {
            var illusts = await _queryCacheService.RunAsync(_pixivClient.User.BrowsingHistory.IllustAsync, offset => _offset);
            illusts?.IllustList.ForEach(w => Illusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        private async Task FetchNovels()
        {
            var novels = await _queryCacheService.RunAsync(_pixivClient.User.BrowsingHistory.NovelAsync, offset => _offset);
            novels?.NovelList.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

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