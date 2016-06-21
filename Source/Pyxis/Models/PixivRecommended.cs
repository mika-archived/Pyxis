using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;

// ReSharper disable InconsistentNaming

namespace Pyxis.Models
{
    internal class PixivRecommended : ISupportIncrementalLoading
    {
        private readonly ContentType _contentType;
        private readonly IPixivClient _pixivClient;

        public ObservableCollection<IIllust> RecommendedImages { get; }
        public ObservableCollection<INovel> RecommendedNovels { get; }

        public PixivRecommended(IPixivClient pixivClient, ContentType contentType)
        {
            _contentType = contentType;
            _pixivClient = pixivClient;
            RecommendedNovels = new ObservableCollection<INovel>();
            RecommendedImages = new ObservableCollection<IIllust>();
        }

        public void Fetch(bool isClear) => RunHelper.RunAsync(FetchRecommended, isClear);

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
                illusts =
                    await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => "illust", filter => "for_ios",
                                                                        offset => Count());
            else if (_contentType == ContentType.Manga)
                illusts =
                    await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => "manga", filter => "for_ios",
                                                                        offset => Count());
            illusts?.Illusts.ForEach(w => RecommendedImages.Add(w));
        }

        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                RecommendedNovels.Clear();
            var novels = await _pixivClient.NovelV1.RecommendedNologinAsync();
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

        public bool HasMoreItems => true;

        #endregion
    }
}