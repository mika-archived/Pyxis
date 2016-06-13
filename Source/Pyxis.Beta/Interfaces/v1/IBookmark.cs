using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     ブックマークユーザー
    /// </summary>
    public interface IBookmark : IIndex
    {
        /// <summary>
        ///     ブックマークしているユーザー
        /// </summary>
        IList<IUser> Users { get; }
    }
}