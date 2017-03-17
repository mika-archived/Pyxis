using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivRanking : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly RankingMode _rankingMode;
        private readonly ContentType _rankingType;
        private int _offset;

        public ObservableCollection<Tuple<RankingMode, IllustsRoot>> Ranking { get; }
        public ObservableCollection<Tuple<RankingMode, NovelsRoot>> RankingOfNovels { get; }
        public ObservableCollection<Illust> Illusts { get; }
        public ObservableCollection<Novel> Novels { get; }

        public PixivRanking(PixivClient pixivClient, ContentType rankingType, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _rankingType = rankingType;
            _queryCacheService = queryCacheService;
            if (_rankingType == ContentType.Novel)
                RankingOfNovels = new ObservableCollection<Tuple<RankingMode, NovelsRoot>>();
            else
                Ranking = new ObservableCollection<Tuple<RankingMode, IllustsRoot>>();
            HasMoreItems = false;
        }

        public PixivRanking(PixivClient pixivClient, ContentType rankingType, RankingMode rankingMode, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _rankingType = rankingType;
            _rankingMode = rankingMode;
            _queryCacheService = queryCacheService;
            _offset = 0;
            if (_rankingType == ContentType.Novel)
                Novels = new ObservableCollection<Novel>();
            else
                Illusts = new ObservableCollection<Illust>();
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
                await FetchIllustsRoot();
        }

        private async Task FetchIllustsRoot()
        {
            var illustsRoot = await _pixivClient.Illust.RankingAsync(Sagitta.Enum.RankingMode.Day, filter: "for_ios", offset: _offset);
            illustsRoot?.Illusts.ForEach(w => Illusts.Add(w));
            if (string.IsNullOrWhiteSpace(illustsRoot?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(illustsRoot.NextUrl)["offset"]);
        }

        private async Task FetchNovels()
        {
            var novels = await _pixivClient.Novel.RankingAsync(Sagitta.Enum.RankingMode.Day, offset: _offset);
            novels?.Novels.ForEach(w => Novels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
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
                var illusts = await _pixivClient.Illust.RankingAsync(Sagitta.Enum.RankingMode.Day, filter: "for_ios");
                Ranking.Add(new Tuple<RankingMode, IllustsRoot>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchMangaRanking()
        {
            Ranking.Clear();
            var modes = new[] {"day", "week_rookie", "week", "month"};
            foreach (var _ in modes)
            {
                var illusts = await _pixivClient.Illust.RankingAsync(Sagitta.Enum.RankingMode.DayManga, filter: "for_ios");
                if (illusts != null)
                    Ranking.Add(new Tuple<RankingMode, IllustsRoot>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchNovelRanking()
        {
            RankingOfNovels.Clear();
            var modes = new[] {"day", "day_male", "day_female", "week_rookie", "week"};
            foreach (var _ in modes)
            {
                var novels = await _pixivClient.Novel.RankingAsync(Sagitta.Enum.RankingMode.Day);
                if (novels != null)
                    RankingOfNovels.Add(new Tuple<RankingMode, NovelsRoot>(RankingModeExt.FromString(_), novels));
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