using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Events;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Beta.Interfaces.Rest
{
    public interface IPixivClient
    {
        /// <summary>
        ///     Wrapper of auth/token
        /// </summary>
        IAuthorizationApi Authorization { get; }

        /// <summary>
        ///     Wrapper of v1/application-info
        /// </summary>
        IApplicationInfoApi ApplicationInfo { get; }

        /// <summary>
        ///     Wrapper of v1/illust
        /// </summary>
        IIllustApi IllustV1 { get; }

        /// <summary>
        ///     Wrapper of v1/manga
        /// </summary>
        IMangaApi Manga { get; }

        /// <summary>
        ///     Wrapper of v1/novel
        /// </summary>
        INovelApi NovelV1 { get; }

        /// <summary>
        ///     Wrapper of v1/search
        /// </summary>
        ISearchApi Search { get; }

        /// <summary>
        ///     Wrapper of v1/spotlight
        /// </summary>
        ISpotlightApi Spotlight { get; }

        /// <summary>
        ///     Wrapper of v1/trending-tags
        /// </summary>
        ITrendingTagsApi TrendingTags { get; }

        /// <summary>
        ///     Wrapper of v1/ugoira
        /// </summary>
        IUgoiraApi Ugoira { get; }

        /// <summary>
        ///     Wrapper of v1/user
        /// </summary>
        IUserApi User { get; }

        /// <summary>
        ///     Wrapper of v2/illust
        /// </summary>
        v2.IIllustApi IllustV2 { get; }

        /// <summary>
        ///     Wrapper of v1/novel
        /// </summary>
        v2.INovelApi NovelV2 { get; }

        /// <summary>
        ///     Wrapper of getting pixiv image
        /// </summary>
        IPximgApi Pximg { get; }

        // =====================
        // HTTP access
        // ---------------------

        Task<T> GetAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters);

        Task<T> PostAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters);

        // =====================
        // Re-authenticate event
        // ---------------------
        event ReAuthenticateEventHandler OnReAuthenticate;
    }
}