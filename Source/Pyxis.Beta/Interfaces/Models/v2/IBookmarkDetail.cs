using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.Models.v2
{
    public interface IBookmarkDetail
    {
        bool IsBookmarked { get; }

        IList<IBookmarkDetailTag> Tags { get; }

        string Restrict { get; }
    }
}