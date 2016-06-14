using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    public class IllustApi : IIllustApi
    {
        private readonly PixivApiClient _client;

        public IllustApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IIllustApi

        public async Task<IIllusts> FollowAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.IllustFollow, true, parameters);

        public async Task<IIllusts> MypixivAsync()
            => await _client.GetAsync<Illusts>(Endpoints.IllustMypixiv, true);

        #endregion
    }
}