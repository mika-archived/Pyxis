using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class TrendingTagsApi : ITrendingTagsApi
    {
        private readonly PixivApiClient _client;

        public TrendingTagsApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of ITrendingTagsApi

        public async Task<ITrendingTags> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<TrendingTags>(Endpoints.TrendingTagsIllust, false, parameters);

        public async Task<ITrendingTags> NovelAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<TrendingTags>(Endpoints.TrendingTagsNovel, false, parameters);

        #endregion
    }
}