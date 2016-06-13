using System;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     コメント
    /// </summary>
    public interface IComment : IIdentity
    {
        /// <summary>
        ///     コメント
        /// </summary>
        string Comment { get; }

        /// <summary>
        ///     投稿時間
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        ///     投稿ユーザー
        /// </summary>
        IUser User { get; }
    }
}