using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Extensions;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivNew : ISupportIncrementalLoading
    {
        private readonly ContentType _contentType;
        private readonly FollowType _followType;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _maxIllustId;
        private int _maxNovelId;
        private int _offset;
        public ObservableCollection<Illust> NewIllustsRoot { get; }
        public ObservableCollection<Novel> NewNovels { get; }

        public PixivNew(ContentType contentType, FollowType followType, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _contentType = contentType;
            _followType = followType;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            NewIllustsRoot = new ObservableCollection<Illust>();
            NewNovels = new ObservableCollection<Novel>();
            _offset = 0;
            _maxNovelId = 0;
            _maxIllustId = 0;
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task FetchNewItems(bool isClear)
        {
            if (_contentType == ContentType.Novel)
                await FetchNovels(isClear);
            else
                await FetchIllustsRoot(isClear);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                NewNovels.Clear();

            NovelsRoot novels = null;
            if (_followType == FollowType.Following)
                novels = await _pixivClient.Novel.FollowAsync(Restrict.All, _offset);
            else if (_followType == FollowType.Mypixiv)
                novels = await _pixivClient.Novel.MypixivAsync(_offset);
            else if (_followType == FollowType.All)
                novels = await _pixivClient.Novel.NewAsync(_maxNovelId);
            novels?.Novels.ForEach(w => NewNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
            {
                HasMoreItems = false;
            }
            else
            {
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl).TryGet("offset"));
                _maxNovelId = int.Parse(UrlParameter.ParseQuery(novels.NextUrl).TryGet("max_novel_id"));
            }
        }

        #region Illust related

        private async Task FetchIllustsRoot(bool isClear)
        {
            if (isClear)
                NewIllustsRoot.Clear();

            IllustsRoot illustsRoot;
            if (_contentType == ContentType.Illust)
                illustsRoot = await FetchIllustsRoot();
            else
                illustsRoot = await FetchManga();
            illustsRoot?.Illusts.ForEach(w => NewIllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illustsRoot?.NextUrl))
            {
                HasMoreItems = false;
            }
            else
            {
                _offset = int.Parse(UrlParameter.ParseQuery(illustsRoot.NextUrl).TryGet("offset"));
                _maxIllustId = int.Parse(UrlParameter.ParseQuery(illustsRoot.NextUrl).TryGet("max_illust_id"));
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task<IllustsRoot> FetchIllustsRoot()
        {
            IllustsRoot illustsRoot = null;
            if (_followType == FollowType.Following)
                illustsRoot = await _pixivClient.Illust.FollowAsync(Restrict.All, offset: _offset);
            else if (_followType == FollowType.Mypixiv)
                illustsRoot = await _pixivClient.Illust.MypixivAsync(_offset);
            else if (_followType == FollowType.All)
                illustsRoot = await _pixivClient.Illust.NewAsync(IllustType.Illust, "for_ios", _maxIllustId);
            return illustsRoot;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task<IllustsRoot> FetchManga()
        {
            IllustsRoot illustsRoot = null;
            if (_followType == FollowType.Following)
                throw new NotSupportedException("Following");
            if (_followType == FollowType.Mypixiv)
                throw new NotSupportedException("Mypixiv");
            if (_followType == FollowType.All)
                illustsRoot = await _pixivClient.Illust.NewAsync(IllustType.Manga, "for_ios", _maxIllustId);
            return illustsRoot;
        }

        #endregion

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchNewItems(false);
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}