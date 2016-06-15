using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class AuthorizationApi : IAuthorizationApi
    {
        private readonly PixivApiClient _client;

        public AuthorizationApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IAuthorizationApi

        public async Task<IResponse> Login(params Expression<Func<string, object>>[] parameters)
        {
            var response = await _client.GetAsync<ResponseOwneer>(Endpoints.OauthToken, false, parameters);
            return response.Response;
        }

        #endregion
    }

    public class ResponseOwneer
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }
}