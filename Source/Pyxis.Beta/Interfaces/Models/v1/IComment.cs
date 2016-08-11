using System;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     コメント
    /// </summary>
    public interface IComment : IIdentity
    {
        /// <summary>
        ///     コメント
        /// </summary>
        string CommentBody { get; }

        /// <summary>
        ///     投稿時間
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        ///     投稿ユーザー
        /// </summary>
        IUserBase User { get; }
    }
}