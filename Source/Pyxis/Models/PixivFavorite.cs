using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivFavorite : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private int _maxBookmarkId;
        private FavoriteOptionParameter _optionParam;

        public ObservableCollection<Illust> ResultIllustsRoot { get; }
        public ObservableCollection<Novel> ResultNovels { get; }

        public PixivFavorite(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            ResultIllustsRoot = new ObservableCollection<Illust>();
            ResultNovels = new ObservableCollection<Novel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Query(FavoriteOptionParameter optionParameter)
        {
            ResultIllustsRoot.Clear();
            ResultNovels.Clear();
            _optionParam = optionParameter;
            // Magic number
            _optionParam.Tag = optionParameter.Tag == "すべて" ? "" : optionParameter.Tag;
            _maxBookmarkId = 0;
#if !OFFLINE
            HasMoreItems = true;
#endif
        }

        private async Task QueryAsync()
        {
            if (_optionParam == null)
            {
                HasMoreItems = false;
                return;
            }
            if (_optionParam.Type == SearchType.IllustsAndManga)
                await QueryIllust();
            else if (_optionParam.Type == SearchType.Novels)
                await QueryNovel();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task QueryIllust()
        {
            var illusts = await _pixivClient.User.Bookmarks.IllustAsync(int.Parse(_optionParam.UserId), "for_ios", restrict: Restrict.Public,
                                                                        maxBookmarkId: _maxBookmarkId, tag: _optionParam.Tag);
            illusts?.Illusts.ForEach(w => ResultIllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _maxBookmarkId = int.Parse(UrlParameter.ParseQuery(illusts.NextUrl)["max_bookmark_id"]);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task QueryNovel()
        {
            var novels = await _pixivClient.User.Bookmarks.NovelAsync(int.Parse(_optionParam.UserId), _maxBookmarkId, Restrict.Public, _optionParam.Tag);
            novels?.Novels.ForEach(w => ResultNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _maxBookmarkId = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["max_bookmark_id"]);
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