using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IRecommendedNovels : IErrorResponse, IIndex
    {
        IList<INovel> Novels { get; }

        IList<INovel> HomeRankingNovels { get; }

        IRankingLabelNovel RankingLabelNovel { get; }
    }
}