﻿using System;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface ISpotlightArticle : IIdentity
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     サムネイル画像 URL
        /// </summary>
        string Thumbnail { get; }

        /// <summary>
        ///     記事 URL
        /// </summary>
        string ArticleUrl { get; }

        /// <summary>
        ///     公開日
        /// </summary>
        DateTime PublishDate { get; }
    }
}