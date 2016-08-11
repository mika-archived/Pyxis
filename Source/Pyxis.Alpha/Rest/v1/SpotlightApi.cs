using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class SpotlightApi : ISpotlightApi
    {
        private readonly PixivApiClient _client;

        public SpotlightApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of ISpotlightApi

        public async Task<ISpotlightArticles> ArticlesAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<SpotlightArticles>(Endpoints.SpotlightArticles, false, parameters);

        #endregion
    }
}