using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IRecommendedIllusts : IErrorResponse, IIndex
    {
        IList<IIllust> Illusts { get; }

        IList<IIllust> HomeRankingIllusts { get; }

        IRankingLabelIllust RankingLabelIllust { get; }
    }
}