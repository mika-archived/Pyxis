using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class MangaApi : IMangaApi
    {
        private readonly PixivApiClient _client;

        public MangaApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IMangaApi

        public async Task<IIllusts> RecommendedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.MangaRecommended, false, parameters);

        #endregion
    }
}