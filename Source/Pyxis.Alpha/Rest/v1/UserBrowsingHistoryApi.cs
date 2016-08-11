using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBrowsingHistoryApi : IUserBrowsingHistoryApi
    {
        private readonly PixivApiClient _client;

        public UserBrowsingHistoryApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBrowsingHistoryApi

        public IUserBrowsingHistoryIllustApi Illust => new UserBrowsingHistoryIllustApi(_client);
        public IUserBrowsingHistoryNovelApi Novel => new UserBrowsingHistoryNovelApi(_client);

        public async Task<IIllusts> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.UserBrowsingHistoryIllusts, true, parameters);

        public async Task<INovels> NovelAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<INovels>(Endpoints.UserBrowsingHistoryNovels, true, parameters);

        #endregion
    }
}