using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivRanking : PixivModel
    {
        public ObservableCollection<Illust> IllustRanking { get; }
        public ObservableCollection<Illust> MangaRanking { get; }
        public ObservableCollection<Novel> NovelRanking { get; }

        public PixivRanking(PixivClient pixivClient) : base(pixivClient)
        {
            IllustRanking = new ObservableCollection<Illust>();
            MangaRanking = new ObservableCollection<Illust>();
            NovelRanking = new ObservableCollection<Novel>();
        }

        public async Task FetchIllustRankingAsync(RankingMode mode, DateTime? date)
        {
            var illusts = await PixivClient.Illust.RankingAsync(mode, date?.ToString("yyyy/MM/dd"));
            IllustRanking.Clear();
            illusts.Illusts.ForEach(w => IllustRanking.Add(w));
        }

        public async Task FetchMangaRankingAsync(RankingMode mode, DateTime? date)
        {
            var manga = await PixivClient.Illust.RankingAsync(mode, date?.ToString("yyyy/MM/dd"));
            MangaRanking.Clear();
            manga.Illusts.ForEach(w => MangaRanking.Add(w));
        }

        public async Task FetchNovelRankingAsync(RankingMode mode, DateTime? date)
        {
            var novels = await PixivClient.Novel.RankingAsync(mode, date?.ToString("yyyy/MM/dd"));
            NovelRanking.Clear();
            novels.Novels.ForEach(w => NovelRanking.Add(w));
        }
    }
}