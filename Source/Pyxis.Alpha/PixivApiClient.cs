using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Rest;
using Pyxis.Alpha.Rest.Pximg;
using Pyxis.Alpha.Rest.v1;
using Pyxis.Beta.Events;
using Pyxis.Beta.Exceptions;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;

using IllustV1 = Pyxis.Beta.Interfaces.Rest.v1.IIllustApi;
using IllustV2 = Pyxis.Beta.Interfaces.Rest.v2.IIllustApi;
using INovelV1 = Pyxis.Beta.Interfaces.Rest.v1.INovelApi;
using INovelV2 = Pyxis.Beta.Interfaces.Rest.v2.INovelApi;
using IUserV1 = Pyxis.Beta.Interfaces.Rest.v1.IUserApi;
using IUserV2 = Pyxis.Beta.Interfaces.Rest.v2.IUserApi;
using IllustApiV1 = Pyxis.Alpha.Rest.v1.IllustApi;
using IllustApiV2 = Pyxis.Alpha.Rest.v2.IllustApi;
using NovelApiV1 = Pyxis.Alpha.Rest.v1.NovelApi;
using NovelApiV2 = Pyxis.Alpha.Rest.v2.NovelApi;
using UserApiV1 = Pyxis.Alpha.Rest.v1.UserApi;
using UserApiV2 = Pyxis.Alpha.Rest.v2.UserApi;

namespace Pyxis.Alpha
{
    /// <summary>
    ///     pixiv への API アクセスを行います。
    /// </summary>
    public class PixivApiClient : IPixivClient
    {
        public string AccessToken { get; internal set; }

        private IList<KeyValuePair<string, string>> GetPrameter(params Expression<Func<string, object>>[] parameters)
            => parameters.Select(w => new KeyValuePair<string, string>(ParameterUtil.Name(w), ParameterUtil.Value(w))).ToList();

        #region Implementation of IPixivClient

        public IAuthorizationApi Authorization => new AuthorizationApi(this);
        public IApplicationInfoApi ApplicationInfo => new ApplicationInfoApi(this);
        public IllustV1 IllustV1 => new IllustApiV1(this);
        public IMangaApi Manga => new MangaApi(this);
        public INovelV1 NovelV1 => new NovelApiV1(this);
        public ISearchApi Search => new SearchApi(this);
        public ISpotlightApi Spotlight => new SpotlightApi(this);
        public ITrendingTagsApi TrendingTags => new TrendingTagsApi(this);
        public IUgoiraApi Ugoira => new UgoiraApi(this);
        public IUserV1 UserV1 => new UserApiV1(this);
        public IllustV2 IllustV2 => new IllustApiV2(this);
        public INovelV2 NovelV2 => new NovelApiV2(this);
        public IPximgApi Pximg => new PximgApi();
        public IUserV2 UserV2 => new UserApiV2(this);

        private readonly HttpClient _httpClient;
        internal readonly string ClientId;
        internal readonly string ClientSecret;

        public PixivApiClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("App-Version", "6.4.0");
            _httpClient.DefaultRequestHeaders.Add("App-OS", "ios");
            _httpClient.DefaultRequestHeaders.Add("App-OS-Version", "10.2");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "PixivIOSApp/6.4.0 (iOS 10.2; iPhone7,2)");
        }

        public async Task<T> GetAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters)
        {
            if (requireAuth && string.IsNullOrWhiteSpace(AccessToken))
                throw new AuthenticateRequiredException();

            var param = string.Join("&", GetPrameter(parameters)
                                             .Where(w => !string.IsNullOrWhiteSpace(w.Value))
                                             .Select(w => $"{w.Key}={Uri.EscapeDataString(w.Value)}"));
            url += "?" + param;
            try
            {
                Debug.WriteLine($"GET  :{url}");
                if (!string.IsNullOrWhiteSpace(AccessToken))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException e) when (e.Message.Contains("400") && url != Endpoints.OauthToken)
            {
                var eventArgs = new ReAuthenticateEventArgs();
                OnReAuthenticate?.Invoke(eventArgs);
                // ReSharper disable InconsistentNaming
                var response = await Authorization.Login(get_secure_url => 1,
                                                         grant_type => "password",
                                                         device_token => eventArgs.DeviceId,
                                                         password => eventArgs.Password,
                                                         username => eventArgs.Username);
                if (response != null)
                    return await GetAsync<T>(url, requireAuth, parameters);
                // ReSharper restore InconsistentNaming
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        public async Task<T> PostAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters)
        {
            if (requireAuth && string.IsNullOrWhiteSpace(AccessToken))
                throw new AuthenticateRequiredException();

            var param = GetPrameter(parameters).Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
            var content = new FormUrlEncodedContent(param);

            try
            {
                Debug.WriteLine($"POST :{url}");
                // Modified DefaultRequestheaders.
                if (!string.IsNullOrWhiteSpace(AccessToken))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException e) when (e.Message.Contains("400") && url != Endpoints.OauthToken)
            {
                var eventArgs = new ReAuthenticateEventArgs();
                OnReAuthenticate?.Invoke(eventArgs);
                // ReSharper disable InconsistentNaming
                var response = await Authorization.Login(get_secure_url => 1,
                                                         grant_type => "password",
                                                         device_token => eventArgs.DeviceId,
                                                         password => eventArgs.Password,
                                                         username => eventArgs.Username);
                if (response != null)
                    return await PostAsync<T>(url, requireAuth, parameters);
                // ReSharper restore InconsistentNaming
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        public event ReAuthenticateEventHandler OnReAuthenticate;

        #endregion Implementation of IPixivClient
    }
}