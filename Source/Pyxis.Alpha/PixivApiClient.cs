using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha
{
    /// <summary>
    ///     pixiv への API アクセスを行います。
    /// </summary>
    public class PixivApiClient : IPixivClient
    {
        #region Implementation of IPixivClient

        public IApplicationInfoApi ApplicationInfo { get; }
        public IIllustApi IllustV1 { get; }
        public IMangaApi Manga { get; }
        public INovelApi Novel { get; }
        public ISearchApi Search { get; }
        public ISpotlightApi Spotlight { get; }
        public ITrendingTagsApi TrendingTags { get; }
        public IUserApi User { get; }
        public Beta.Interfaces.Rest.v2.IIllustApi IllustV2 { get; }

        public Task<T> GetAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}