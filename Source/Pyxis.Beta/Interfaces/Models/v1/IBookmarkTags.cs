using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IBookmarkTags : IErrorResponse, IIndex
    {
        IList<IBookmarkTag> BookmarkTagList { get; }
    }
}