using System;
using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface INovel : IIdentity
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     キャプション
        /// </summary>
        string Caption { get; }

        /// <summary>
        ///     年齢制限
        /// </summary>
        int Restrict { get; }

        /// <summary>
        ///     画像 URL
        /// </summary>
        IImageUrls ImageUrls { get; }

        /// <summary>
        ///     投稿日
        /// </summary>
        DateTime CreateDate { get; }

        /// <summary>
        ///     タグ
        /// </summary>
        IList<ITag> Tags { get; }

        /// <summary>
        ///     ページ数
        /// </summary>
        int PageCount { get; }

        /// <summary>
        ///     文字数
        /// </summary>
        int TextLength { get; }

        /// <summary>
        ///     ユーザー
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     シリーズ情報
        /// </summary>
        ISeries Series { get; }

        /// <summary>
        ///     ブックマーク状態
        /// </summary>
        bool IsBookmarked { get; }

        /// <summary>
        ///     ブックマーク数
        /// </summary>
        int TotalBookmarks { get; }

        /// <summary>
        ///     閲覧数
        /// </summary>
        int TotalView { get; }

        bool Visible { get; }

        /// <summary>
        ///     コメント数
        /// </summary>
        int TotalComments { get; }
    }
}