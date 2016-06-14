using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class NovelApi : INovelApi
    {
        private readonly PixivApiClient _client;

        public NovelApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelApi

        public async Task<INovels> FollowAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelFollow, true, parameters);

        public async Task<INovels> MypixivAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelMypixiv, true, parameters);

        public async Task<INovels> NewAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelNew, false, parameters);

        public async Task<INovels> RankingAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelRanking, false, parameters);

        public async Task<IRecommendedNovels> RecommendedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<RecommendedNovels>(Endpoints.NovelRecommended, true, parameters);

        public async Task<IRecommendedNovels> RecommendedNologinAsync(
            params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<RecommendedNovels>(Endpoints.NovelRecommendedNoLogin, false, parameters);

        #endregion
    }
}