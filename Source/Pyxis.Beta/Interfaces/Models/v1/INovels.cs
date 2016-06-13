using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface INovels : IIndex
    {
        IList<INovel> NovelList { get; }
    }
}