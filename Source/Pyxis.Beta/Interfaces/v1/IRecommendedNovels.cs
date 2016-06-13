using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface IRecommendedNovels : IIndex
    {
        IList<INovel> Novels { get; }

        IList<INovel> HomeRankingNovels { get; }

        IRankingLabelNovel RankingLabelNovel { get; }
    }
}