using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface IRecommendedIllusts : IIndex
    {
        IList<IIllust> Illusts { get; }

        IList<IIllust> HomeRankingIllusts { get; }

        IRankingLabelIllust RankingLabelIllust { get; }
    }
}