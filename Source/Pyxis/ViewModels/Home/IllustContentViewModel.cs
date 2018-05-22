using System.Threading.Tasks;

using Pyxis.Extensions;
using Pyxis.Models.Pixiv;
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

        public IllustContentViewModel(PixivClient pixivClient)
        {
            _ranking = new PixivRanking(pixivClient);
            RankingIllusts = _ranking.IllustRanking.ToReadOnlyReactiveCollection(w => new IllustViewModel(w)).AddTo(this);
        }

        public override async Task InitializeAsync()
        {
            await _ranking.FetchIllustRankingAsync(RankingMode.Daily, null);
        }
    }
}