using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBrowsingHistoryIllustApi : IUserBrowsingHistoryIllustApi
    {
        private readonly PixivApiClient _client;

        public UserBrowsingHistoryIllustApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBrowsingHistoryIllustApi

        public async Task AddAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<Task>(Endpoints.UserBrowsingHistoryIllustAdd, true, parameters);

        #endregion
    }
}