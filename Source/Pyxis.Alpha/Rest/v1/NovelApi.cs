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

        public INovelBookmarkApi Bookmark => new NovelBookmarkApi(_client);
        public INovelMarkerApi Marker => new NovelMarkerApi(_client);

        public async Task<IComments> CommentsAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Comments>(Endpoints.NovelComments, false, parameters);

        public async Task<INovels> FollowAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelFollow, true, parameters);

        public async Task<INovels> MarkersAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelMarkers, true, parameters);

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

        public async Task<INovels> SeriesAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.NovelSeries, false, parameters);

        public async Task<IText> TextAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Text>(Endpoints.NovelText, false, parameters);

        #endregion
    }
}