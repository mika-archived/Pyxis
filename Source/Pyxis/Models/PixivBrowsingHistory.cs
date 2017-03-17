using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivBrowsingHistory : ISupportIncrementalLoading
    {
        private readonly ContentType2 _contentType;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _offset;

        public ObservableCollection<Illust> IllustsRoot { get; }
        public ObservableCollection<Novel> Novels { get; }

        public PixivBrowsingHistory(PixivClient pixivClient, ContentType2 contentType, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _contentType = contentType;
            _queryCacheService = queryCacheService;
            _offset = 0;
            IllustsRoot = new ObservableCollection<Illust>();
            Novels = new ObservableCollection<Novel>();
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
                await FetchIllustsRoot();
            else
                await FetchNovels();
            ;
        }

        private async Task FetchIllustsRoot()
        {
            var illusts = await _pixivClient.User.BrowsingHistory.IllustsAsync(_offset);
            illusts?.Illusts.ForEach(w => IllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(illusts?.NextUrl)["offset"]);
        }

        private async Task FetchNovels()
        {
            var novels = await _pixivClient.User.BrowsingHistory.NovelsAsync(_offset);
            novels?.Novels.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
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