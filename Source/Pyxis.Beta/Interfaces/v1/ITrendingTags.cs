using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface ITrendingTags
    {
        IList<ITrendTag> TrendTags { get; }
    }
}