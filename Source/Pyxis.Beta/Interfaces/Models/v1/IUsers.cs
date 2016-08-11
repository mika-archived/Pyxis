using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     ブックマークユーザー
    /// </summary>
    public interface IUsers : IErrorResponse, IIndex
    {
        /// <summary>
        ///     ブックマークしているユーザー
        /// </summary>
        IList<IUser> UserList { get; }
    }
}