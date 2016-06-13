using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface IUserPreview
    {
        /// <summary>
        ///     ユーザー
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     イラスト
        /// </summary>
        IList<IIllust> Illusts { get; }

        /// <summary>
        ///     小説
        /// </summary>
        IList<INovel> Novels { get; }
    }
}