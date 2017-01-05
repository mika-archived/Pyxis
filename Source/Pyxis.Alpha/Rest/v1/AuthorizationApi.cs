using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

// ReSharper disable InconsistentNaming

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
            var modifiParams = parameters.ToList();
            if (modifiParams.All(w => w.Name != "client_id"))
                modifiParams.Add(client_id => _client.ClientId);
            if (modifiParams.All(w => w.Name != "client_secret"))
                modifiParams.Add(client_secret => _client.ClientSecret);

            var response = await _client.PostAsync<ResponseOwneer>(Endpoints.OauthToken, false, modifiParams.ToArray());
            if (response != null)
                _client.AccessToken = response.Response.AccessToken;
            return response?.Response;
        }

        #endregion
    }

    public class ResponseOwneer
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }
}