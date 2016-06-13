using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     コメント
    /// </summary>
    public interface IComments : IIndex
    {
        /// <summary>
        ///     コメント数
        /// </summary>
        int TotalComments { get; }

        IList<IComment> Comments { get; }
    }
}