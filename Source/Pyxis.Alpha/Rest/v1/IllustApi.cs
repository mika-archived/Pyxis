using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class IllustApi : IIllustApi
    {
        private readonly PixivApiClient _client;

        public IllustApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IIllustApi

        public IIllustBookmarkApi Bookmark => new IllustBookmarkApi(_client);

        public async Task<IComments> CommentsAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Comments>(Endpoints.IllustComments, false, parameters);

        public async Task<IIllusts> NewAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.IllustNew, false, parameters);

        public async Task<IIllusts> RankingAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.IllustRanking, false, parameters);

        public async Task<IRecommendedIllusts> RecommendedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<RecommendedIllusts>(Endpoints.IllustRecommended, true, parameters);

        public async Task<IRecommendedIllusts> RecommendedNologinAsync(
            params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<RecommendedIllusts>(Endpoints.IllustRecommendedNoLogin, false, parameters);

        public async Task<IIllusts> RelatedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.IllustRelated, false, parameters);

        #endregion
    }
}