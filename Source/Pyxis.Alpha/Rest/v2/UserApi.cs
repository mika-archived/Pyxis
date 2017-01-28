using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    public class UserApi : IUserApi
    {
        private readonly PixivApiClient _client;

        public UserApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserApi

        public async Task<IUsers> ListAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Users>(Endpoints.UserList, false, parameters);

        public IUserBrowsingHistoryApi BrowsingHistory => new UserBrowsingHistoryApi(_client);

        #endregion
    }
}