using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Pyxis.Extensions;
using Pyxis.Models.Pixiv;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;

using Sagitta;
using Sagitta.Enum;

namespace Pyxis.ViewModels.Home
{
    public class IllustContentViewModel : HomeContentViewModel
    {
        private readonly PixivRanking _ranking;
        public ReadOnlyReactiveCollection<IllustViewModel> RankingIllusts { get; }
        public IncrementalLoadingCollection<IllustRecommendSource<IllustViewModel>, IllustViewModel> RecommendIllusts { get; }

        public IllustContentViewModel(PixivClient pixivClient, IObjectCacheStorage objectCacheStorage)
        {
            _ranking = new PixivRanking(pixivClient);
            RankingIllusts = _ranking.IllustRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
            RecommendIllusts = new IncrementalLoadingCollection<IllustRecommendSource<IllustViewModel>, IllustViewModel>(
                new IllustRecommendSource<IllustViewModel>(pixivClient, objectCacheStorage, IllustType.Illust, w => new IllustViewModel(w)));
        }

        public override async Task InitializeAsync()
        {
            await _ranking.FetchIllustRankingAsync(RankingMode.Daily, null);
        }
    }
}