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

// ReSharper disable InconsistentNaming

namespace Pyxis.Models
{
    internal class PixivRecommended : ISupportIncrementalLoading
    {
        private readonly IAccountService _accountService;
        private readonly ContentType _contentType;
        private readonly IPixivClient _pixivClient;
        private string _offset;

        public ObservableCollection<IIllust> RecommendedImages { get; }
        public ObservableCollection<INovel> RecommendedNovels { get; }

        public PixivRecommended(IAccountService accountService, IPixivClient pixivClient, ContentType contentType)
        {
            _accountService = accountService;
            _contentType = contentType;
            _pixivClient = pixivClient;
            _offset = "";
            RecommendedNovels = new ObservableCollection<INovel>();
            RecommendedImages = new ObservableCollection<IIllust>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        private async Task FetchRecommended(bool isClear)
        {
            if (_contentType == ContentType.Novel)
                await FetchNovels(isClear);
            else
                await FetchIllusts(isClear);
        }

        private async Task FetchIllusts(bool isClear)
        {
            if (isClear)
                RecommendedImages.Clear();
            IRecommendedIllusts illusts = null;
            if (_contentType == ContentType.Illust)
                illusts = await RecommendedAsync("illust");
            else if (_contentType == ContentType.Manga)
                illusts = await RecommendedAsync("manga");
            illusts?.Illusts.ForEach(w => RecommendedImages.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        private async Task<IRecommendedIllusts> RecommendedAsync(string contentType)
        {
            // manga/recommended との違いがわからない。
            if (_accountService.IsLoggedIn)
                return await _pixivClient.IllustV1.RecommendedAsync(content_type => contentType,
                                                                    filter => "for_ios",
                                                                    offset => _offset);
            return await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => contentType,
                                                                       filter => "for_ios",
                                                                       offset => _offset);
        }

        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                RecommendedNovels.Clear();
            IRecommendedNovels novels;
            if (_accountService.IsLoggedIn)
                novels = await _pixivClient.NovelV1.RecommendedAsync(offset => _offset);
            else
                novels = await _pixivClient.NovelV1.RecommendedNologinAsync(offset => _offset);
            novels?.Novels.ForEach(w => RecommendedNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchRecommended(false);
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}