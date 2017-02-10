using System;
using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     イラスト
    /// </summary>
    public interface IIllust : IErrorResponse, IIdentity
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     イラストタイプ
        /// </summary>
        string Type { get; }

        /// <summary>
        ///     画像 URL
        /// </summary>
        IImageUrls ImageUrls { get; }

        /// <summary>
        ///     キャプション
        /// </summary>
        string Caption { get; }

        /// <summary>
        ///     年齢制限
        /// </summary>
        int Restrict { get; }

        /// <summary>
        ///     ユーザー
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     タグ
        /// </summary>
        IList<ITag> Tags { get; }

        /// <summary>
        ///     ツール
        /// </summary>
        IList<string> Tools { get; }

        /// <summary>
        ///     投稿日
        /// </summary>
        DateTime CreateDate { get; }

        /// <summary>
        ///     ページ数
        /// </summary>
        int PageCount { get; }

        /// <summary>
        ///     横幅
        /// </summary>
        int Width { get; }

        /// <summary>
        ///     高さ
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     不明
        /// </summary>
        int SanityLevel { get; }

        /// <summary>
        ///     画像(PageCount == 1)
        /// </summary>
        IImageUrls MetaSinglePage { get; }

        /// <summary>
        ///     画像(PageCount > 1)
        /// </summary>
        IList<IMetaPage> MetaPages { get; }

        /// <summary>
        ///     閲覧数
        /// </summary>
        int TotalView { get; }

        /// <summary>
        ///     ブックマーク数
        /// </summary>
        int TotalBookmarks { get; }

        /// <summary>
        ///     ブックマーク状態
        /// </summary>
        bool IsBookmarked { get; }

        //不明
        bool Visible { get; }

        /// <summary>
        ///     コメント数
        /// </summary>
        int TotalComments { get; }

        /// <summary>
        ///     ミュート状態
        /// </summary>
        bool IsMuted { get; }
    }
}