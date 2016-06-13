using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     イラスト
    /// </summary>
    public interface IIllusts : IIndex
    {
        /// <summary>
        ///     イラスト
        /// </summary>
        IList<IIllust> IllustList { get; }
    }
}