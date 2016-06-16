using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Gamma
{
    public class PixivWebClient : IPixivClient
    {
        #region Implementation of IPixivClient

        public IAuthorizationApi Authorization { get; }
        public IApplicationInfoApi ApplicationInfo { get; }
        public IIllustApi IllustV1 { get; }
        public IMangaApi Manga { get; }
        public INovelApi NovelV1 { get; }
        public INovelApi Novel { get; }
        public ISearchApi Search { get; }
        public ISpotlightApi Spotlight { get; }
        public ITrendingTagsApi TrendingTags { get; }
        public IUgoiraApi Ugoira { get; }
        public IUserApi User { get; }
        public Beta.Interfaces.Rest.v2.IIllustApi IllustV2 { get; }
        public Beta.Interfaces.Rest.v2.INovelApi NovelV2 { get; }
        public IPximgApi Pximg { get; }

        public Task<T> GetAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}