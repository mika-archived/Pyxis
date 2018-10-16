using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Models.Pixiv;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;

using Sagitta;
using Sagitta.Enum;

namespace Pyxis.ViewModels.Home
{
    public class MangaContentViewModel : HomeContentViewModel
    {
        private readonly PixivRanking _ranking;
        public ReadOnlyReactiveCollection<IllustViewModel> RankingMangas { get; }
        public IncrementalLoadingCollection<MangaRecommendSource<IllustViewModel>, IllustViewModel> RecommendMangas { get; }

        public MangaContentViewModel(PixivClient pixivClient, INavigationService navigationService, IObjectCacheStorage objectCacheStorage)
        {
            _ranking = new PixivRanking(pixivClient, objectCacheStorage);
            RankingMangas = _ranking.MangaRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w, navigationService)).AddTo(this);
            RecommendMangas = new IncrementalLoadingCollection<MangaRecommendSource<IllustViewModel>, IllustViewModel>(
                new MangaRecommendSource<IllustViewModel>(pixivClient, objectCacheStorage, w => new IllustViewModel(w, navigationService)));
        }

        public override async Task InitializeAsync()
        {
            await _ranking.FetchMangaRankingAsync(RankingMode.DailyManga, null);
        }
    }
}