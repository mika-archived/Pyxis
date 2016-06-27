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

        public ObservableCollection<IIllust> RecommendedImages { get; }
        public ObservableCollection<INovel> RecommendedNovels { get; }

        public PixivRecommended(IAccountService accountService, IPixivClient pixivClient, ContentType contentType)
        {
            _accountService = accountService;
            _contentType = contentType;
            _pixivClient = pixivClient;
            RecommendedNovels = new ObservableCollection<INovel>();
            RecommendedImages = new ObservableCollection<IIllust>();
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
        }

        private async Task<IRecommendedIllusts> RecommendedAsync(string contentType)
        {
            // manga/recommended との違いがわからない。
            if (_accountService.IsLoggedIn)
                return await _pixivClient.IllustV1.RecommendedAsync(content_type => contentType,
                                                                    filter => "for_ios",
                                                                    offset => Count());
            return await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => contentType,
                                                                       filter => "for_ios",
                                                                       offset => Count());
        }

        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                RecommendedNovels.Clear();
            IRecommendedNovels novels;
            if (_accountService.IsLoggedIn)
                novels = await _pixivClient.NovelV1.RecommendedAsync(offset => Count());
            else
                novels = await _pixivClient.NovelV1.RecommendedNologinAsync(offset => Count());
            novels.Novels.ForEach(w => RecommendedNovels.Add(w));
        }

        private int Count()
        {
            if (_contentType == ContentType.Novel)
                return RecommendedNovels.Count;
            return RecommendedImages.Count;
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

#if OFFLINE
        public bool HasMoreItems => false;
#else
        public bool HasMoreItems => true;
#endif

        #endregion
    }
}