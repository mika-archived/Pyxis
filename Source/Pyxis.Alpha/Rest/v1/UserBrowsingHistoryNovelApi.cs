using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBrowsingHistoryNovelApi : IUserBrowsingHistoryNovelApi
    {
        private readonly PixivApiClient _client;

        public UserBrowsingHistoryNovelApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBrowsingHistoryNovelApi

        public async Task AddAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<Task>(Endpoints.UserBrowsingHistoryNovelAdd, true, parameters);

        #endregion
    }
}