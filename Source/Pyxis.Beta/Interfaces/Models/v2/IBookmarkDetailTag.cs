using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Models.v2
{
    public interface IBookmarkDetailTag : ITag
    {
        bool IsRegistered { get; }
    }
}