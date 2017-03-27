using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

using WinRTXamlToolkit.Tools;

namespace Pyxis.Models.Pixiv
{
    /// <summary>
    ///     Get first page of ranking.
    ///     If you need to get second or later pages, use <see cref="PixivRankingSource{T}" />.
    /// </summary>
    internal class PixivRanking : PixivModel
    {
        public ObservableCollection<Illust> IllustRanking { get; }
        public ObservableCollection<Illust> MangaRanking { get; }
        public ObservableCollection<Novel> NovelRanking { get; }

        // Count of Objects that insert to `*Ranking`. Max is 20.
        public int Pickup { get; set; } = 20;

        public PixivRanking(PixivClient pixivClient) : base(pixivClient)
        {
            IllustRanking = new ObservableCollection<Illust>();
            MangaRanking = new ObservableCollection<Illust>();
            NovelRanking = new ObservableCollection<Novel>();
        }

        public async Task FetchIllustRankingAsync(RankingMode mode, DateTime? date)
        {
            IllustRanking.Clear();
            var ranking = await PixivClient.Illust.RankingAsync(mode, date);
            ranking.Illusts.Take(Pickup).ForEach(w => IllustRanking.Add(w));
        }

        public async Task FetchMangaRankingAsync(RankingMode mode, DateTime? date)
        {
            MangaRanking.Clear();
            var ranking = await PixivClient.Illust.RankingAsync(mode, date);
            ranking.Illusts.Take(Pickup).ForEach(w => MangaRanking.Add(w));
        }

        public async Task FetchNovelRankingAsync(RankingMode mode, DateTime? date)
        {
            NovelRanking.Clear();
            var ranking = await PixivClient.Novel.RankingAsync(mode, date);
            ranking.Novels.Take(Pickup).ForEach(w => NovelRanking.Add(w));
        }
    }
}