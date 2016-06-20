using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;

namespace Pyxis.Models
{
    internal class PixivRanking
    {
        private readonly IPixivClient _pixivClient;
        private readonly ContentType _rankingType;

        public ObservableCollection<Tuple<RankingMode, IIllusts>> Ranking { get; }
        public ObservableCollection<Tuple<RankingMode, INovels>> RankingOfNovels { get; }

        public PixivRanking(IPixivClient pixivClient, ContentType rankingType)
        {
            _pixivClient = pixivClient;
            _rankingType = rankingType;
            if (_rankingType == ContentType.Novel)
                RankingOfNovels = new ObservableCollection<Tuple<RankingMode, INovels>>();
            else
                Ranking = new ObservableCollection<Tuple<RankingMode, IIllusts>>();
        }

        public void Fetch() => RunHelper.RunAsync(FetchRanking);

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
                var illusts = await _pixivClient.IllustV1.RankingAsync(mode => _, filter => "for_ios");
                Ranking.Add(new Tuple<RankingMode, IIllusts>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchMangaRanking()
        {
            Ranking.Clear();
            var modes = new[] {"day", "week_rookie", "week", "month"};
            foreach (var _ in modes)
            {
                var illusts = await _pixivClient.IllustV1.RankingAsync(mode => $"{_}_manga");
                Ranking.Add(new Tuple<RankingMode, IIllusts>(RankingModeExt.FromString(_), illusts));
            }
        }

        private async Task FetchNovelRanking()
        {
            RankingOfNovels.Clear();
            var modes = new[] {"day", "day_male", "day_female", "week_rookie", "week"};
            foreach (var _ in modes)
            {
                var novels = await _pixivClient.NovelV1.RankingAsync(mode => _);
                RankingOfNovels.Add(new Tuple<RankingMode, INovels>(RankingModeExt.FromString(_), novels));
            }
        }
    }
}