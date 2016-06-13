using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface ISeries : IIdentity
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }
    }
}