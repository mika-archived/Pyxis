using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface ISeries : IIdentity
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }
    }
}