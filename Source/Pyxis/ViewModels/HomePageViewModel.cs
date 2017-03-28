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

using IllustRecommendSource = Pyxis.Models.Pixiv.PixivRecommendSource<Sagitta.Models.Illust, Pyxis.ViewModels.Contents.IllustViewModel>;
using NovelRecommendSource = Pyxis.Models.Pixiv.PixivRecommendSource<Sagitta.Models.Novel, Pyxis.ViewModels.Contents.NovelViewModel>;

namespace Pyxis.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<IllustViewModel> IllustRanking { get; }
        public ReadOnlyReactiveCollection<IllustViewModel> MangaRanking { get; }
        public ReadOnlyReactiveCollection<NovelViewModel> NovelRanking { get; }
        public IncrementalLoadingCollection<IllustRecommendSource, IllustViewModel> RecommendIllusts { get; }
        public IncrementalLoadingCollection<IllustRecommendSource, IllustViewModel> RecommendMangas { get; }
        public IncrementalLoadingCollection<NovelRecommendSource, NovelViewModel> RecommendNovels { get; }
        public ReactiveProperty<int> SelectedIndex { get; }

        public HomePageViewModel(PixivClient client)
        {
            var pixivRanking = new PixivRanking(client) {Pickup = 5};
            IllustRanking = pixivRanking.IllustRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
            MangaRanking = pixivRanking.MangaRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
            NovelRanking = pixivRanking.NovelRanking.ToReadOnlyReactiveCollection(w => new NovelViewModel(w)).AddTo(this);
            RecommendIllusts = new IncrementalLoadingCollection<IllustRecommendSource, IllustViewModel>(
                new IllustRecommendSource(client, IllustType.Illust, w => new IllustViewModel(w)));
            RecommendMangas = new IncrementalLoadingCollection<IllustRecommendSource, IllustViewModel>(
                new IllustRecommendSource(client, IllustType.Manga, w => new IllustViewModel(w)));
            RecommendNovels = new IncrementalLoadingCollection<NovelRecommendSource, NovelViewModel>(
                new NovelRecommendSource(client, converter: w => new NovelViewModel(w)));
            SelectedIndex = new ReactiveProperty<int>();
            SelectedIndex.SelectMany(async w =>
            {
                switch (w)
                {
                    case 0:
                        await pixivRanking.FetchIllustRankingAsync(RankingMode.Day, null);
                        break;

                    case 1:
                        await pixivRanking.FetchMangaRankingAsync(RankingMode.DayManga, null);
                        break;

                    case 2:
                        await pixivRanking.FetchNovelRankingAsync(RankingMode.Day, null);
                        break;

                    default:
                        throw new NotSupportedException("Wrong Index Number of Pivot.");
                }
                return Unit.Default;
            }).Subscribe().AddTo(this);
        }
    }
}