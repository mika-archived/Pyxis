using System;
using System.Reactive;
using System.Reactive.Linq;

using Microsoft.Toolkit.Uwp;

using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

using IllustRecommendSource = Pyxis.Models.Pixiv.PixivRecommendSource<Sagitta.Models.Illust>;
using NovelRecommendSource = Pyxis.Models.Pixiv.PixivRecommendSource<Sagitta.Models.Novel>;

namespace Pyxis.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        private readonly PixivRanking _pixivRanking;
        public ReadOnlyReactiveCollection<IllustViewModel> IllustRanking { get; }
        public ReadOnlyReactiveCollection<IllustViewModel> MangaRanking { get; }
        public ReadOnlyReactiveCollection<NovelViewModel> NovelRanking { get; }
        public IncrementalLoadingCollection<IllustRecommendSource, Illust> RecommendIllusts { get; }
        public IncrementalLoadingCollection<IllustRecommendSource, Illust> RecommendMangas { get; }
        public IncrementalLoadingCollection<NovelRecommendSource, Novel> RecommendNovels { get; }
        public ReactiveCommand<int> SelectionChangedCommand { get; }

        public HomePageViewModel(PixivClient client)
        {
            _pixivRanking = new PixivRanking(client) {Pickup = 5};
            IllustRanking = _pixivRanking.IllustRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
            MangaRanking = _pixivRanking.MangaRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
            NovelRanking = _pixivRanking.NovelRanking.ToReadOnlyReactiveCollection(w => new NovelViewModel(w)).AddTo(this);
            // RecommendIllusts = new IncrementalLoadingCollection<IllustRecommendSource, Illust>(new IllustRecommendSource(client, IllustType.Illust));
            // RecommendMangas = new IncrementalLoadingCollection<IllustRecommendSource, Illust>(new IllustRecommendSource(client, IllustType.Manga));
            // RecommendNovels = new IncrementalLoadingCollection<NovelRecommendSource, Novel>(new NovelRecommendSource(client));
            SelectionChangedCommand = new ReactiveCommand<int>();
            SelectionChangedCommand.SelectMany(async w =>
            {
                switch (w)
                {
                    case 0:
                        await _pixivRanking.FetchIllustRankingAsync(RankingMode.Day, null);
                        break;

                    case 1:
                        await _pixivRanking.FetchMangaRankingAsync(RankingMode.DayManga, null);
                        break;

                    case 2:
                        await _pixivRanking.FetchNovelRankingAsync(RankingMode.Day, null);
                        break;

                    default:
                        throw new NotSupportedException("Wrong Index Number of Pivot.");
                }
                return Unit.Default;
            }).Subscribe().AddTo(this);
        }
    }
}