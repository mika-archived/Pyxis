using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;

// ReSharper disable InconsistentNaming

namespace Pyxis.Models
{
    internal class PixivRecommended
    {
        private readonly ContentType _contentType;
        private readonly IPixivClient _pixivClient;

        public ObservableCollection<IIllust> RecommendedImages { get; }
        public ObservableCollection<INovel> RecommendedNovels { get; }

        public PixivRecommended(IPixivClient pixivClient, ContentType contentType)
        {
            _contentType = contentType;
            _pixivClient = pixivClient;

            if (_contentType == ContentType.Novel)
                RecommendedNovels = new ObservableCollection<INovel>();
            else
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
                    await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => "illust", filter => "for_ios");
            else if (_contentType == ContentType.Manga)
                illusts =
                    await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => "manga", filter => "for_ios");
            illusts?.Illusts.ForEach(w => RecommendedImages.Add(w));
        }

        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                RecommendedNovels.Clear();
            var novels = await _pixivClient.NovelV1.RecommendedNologinAsync();
            novels.Novels.ForEach(w => RecommendedNovels.Add(w));
        }
    }
}