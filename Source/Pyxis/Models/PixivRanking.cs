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
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivRanking : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly RankingMode _rankingMode;
        private readonly ContentType _rankingType;
        private string _offset;

        public ObservableCollection<Tuple<RankingMode, IIllusts>> Ranking { get; }
        public ObservableCollection<Tuple<RankingMode, INovels>> RankingOfNovels { get; }
        public ObservableCollection<IIllust> Illusts { get; }
        public ObservableCollection<INovel> Novels { get; }

        public PixivRanking(IPixivClient pixivClient, ContentType rankingType, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _rankingType = rankingType;
            _queryCacheService = queryCacheService;
            if (_rankingType == ContentType.Novel)
                RankingOfNovels = new ObservableCollection<Tuple<RankingMode, INovels>>();
            else
                Ranking = new ObservableCollection<Tuple<RankingMode, IIllusts>>();
            HasMoreItems = false;
        }

        public PixivRanking(IPixivClient pixivClient, ContentType rankingType, RankingMode rankingMode, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _rankingType = rankingType;
            _rankingMode = rankingMode;
            _queryCacheService = queryCacheService;
            _offset = "";
            if (_rankingType == ContentType.Novel)
                Novels = new ObservableCollection<INovel>();
            else
                Illusts = new ObservableCollection<IIllust>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        #region Single category

        private async Task FetchAsync()
        {
            if (_rankingType == ContentType.Novel)
                await FetchNovels();
            else
                await FetchIllusts();
        }

        private async Task FetchIllusts()
        {
            var illusts = await _queryCacheService.RunAsync(_pixivClient.IllustV1.RankingAsync,
                                                            mode => _rankingMode.ToParamString(_rankingType),
                                                            filter => "for_ios",
                                                            offset => _offset);
            illusts?.IllustList.ForEach(w => Illusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        private async Task FetchNovels()
        {
            var novels = await _queryCacheService.RunAsync(_pixivClient.NovelV1.RankingAsync,
                                                           mode => _rankingMode.ToParamString(_rankingType),
                                                           offset => _offset);
            novels?.NovelList.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

        #endregion

        #region for All categories

        public void FetchAll() => RunHelper.RunAsync(FetchRanking);

        private async Task FetchRanking()
        {
            if (_rankingType == ContentType.Novel)
                await FetchNovelRanking();
            else if (_rankingType == ContentType.Illust)
                await FetchIllustRanking();
            else if (_rankingType == ContentType.Manga)
                await FetchMangaRanking();
        }

        private async Task FetchIllustRanking()
        {
            Ranking.Clear();
            var modes = new[] {"day", "day_male", "day_female", "week_original", "week_rookie", "week", "month"};
            foreach (var _ in modes)
            {
                var illusts = await _queryCacheService.RunAsync(_pixivClient.IllustV1.RankingAsync, mode => _, filter => "for_ios");
                if (illusts != null)
                    Ranking.Add(new Tuple<RankingMode, IIllusts>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchMangaRanking()
        {
            Ranking.Clear();
            var modes = new[] {"day", "week_rookie", "week", "month"};
            foreach (var _ in modes)
            {
                var illusts = await _queryCacheService.RunAsync(_pixivClient.IllustV1.RankingAsync, mode => $"{_}_manga");
                if (illusts != null)
                    Ranking.Add(new Tuple<RankingMode, IIllusts>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchNovelRanking()
        {
            RankingOfNovels.Clear();
            var modes = new[] {"day", "day_male", "day_female", "week_rookie", "week"};
            foreach (var _ in modes)
            {
                var novels = await _queryCacheService.RunAsync(_pixivClient.NovelV1.RankingAsync, mode => _);
                if (novels != null)
                    RankingOfNovels.Add(new Tuple<RankingMode, INovels>(RankingModeExt.FromString(_), novels));
            }
        }

        #endregion

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