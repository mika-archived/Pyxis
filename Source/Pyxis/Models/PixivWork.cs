using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivWork : ISupportIncrementalLoading
    {
        private readonly ContentType _contentType;
        private readonly string _id;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _offset;

        public ObservableCollection<Illust> IllustsRoot { get; }
        public ObservableCollection<Novel> Novels { get; }

        public PixivWork(string id, ContentType contentType, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _id = id;
            _contentType = contentType;
            _pixivClient = pixivClient;
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

        private async Task FetchAsync()
        {
            if (_contentType == ContentType.Illust)
                await FetchIllustsRoot("illust");
            else if (_contentType == ContentType.Manga)
                await FetchIllustsRoot("manga");
            else if (_contentType == ContentType.Novel)
                await FetchNovels();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchIllustsRoot(string contentType)
        {
            var illusts = await _pixivClient.User.IllustsAsync(IllustType.Illust, int.Parse(_id), "for_ios", _offset);
            illusts?.Illusts.ForEach(w => IllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(illusts.NextUrl)["offset"]);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchNovels()
        {
            var novels = await _pixivClient.User.NovelsAsync(int.Parse(_id), _offset);
            novels?.Novels.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
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