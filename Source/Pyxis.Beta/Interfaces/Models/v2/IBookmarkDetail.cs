using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v2
{
    public interface IBookmarkDetail : IErrorResponse
    {
        bool IsBookmarked { get; }

        IList<IBookmarkDetailTag> Tags { get; }

        string Restrict { get; }
    }
}