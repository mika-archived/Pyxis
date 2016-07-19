using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     コメント
    /// </summary>
    public interface IComments : IErrorResponse, IIndex
    {
        /// <summary>
        ///     コメント数
        /// </summary>
        int TotalComments { get; }

        IList<IComment> CommentList { get; }
    }
}