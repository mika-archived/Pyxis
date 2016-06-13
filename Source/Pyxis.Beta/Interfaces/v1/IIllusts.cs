using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     イラスト
    /// </summary>
    public interface IIllusts : IIndex
    {
        /// <summary>
        ///     イラスト
        /// </summary>
        IList<IIllust> Illusts { get; }
    }
}