using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Rest.Pximg;
using Pyxis.Alpha.Rest.v1;
using Pyxis.Beta.Exceptions;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;

using IllustV1 = Pyxis.Beta.Interfaces.Rest.v1.IIllustApi;
using IllustV2 = Pyxis.Beta.Interfaces.Rest.v2.IIllustApi;
using INovelV1 = Pyxis.Beta.Interfaces.Rest.v1.INovelApi;
using INovelV2 = Pyxis.Beta.Interfaces.Rest.v2.INovelApi;
using IllustApiV1 = Pyxis.Alpha.Rest.v1.IllustApi;
using IllustApiV2 = Pyxis.Alpha.Rest.v2.IllustApi;
using NovelApiV1 = Pyxis.Alpha.Rest.v1.NovelApi;
using NovelApiV2 = Pyxis.Alpha.Rest.v2.NovelApi;

namespace Pyxis.Alpha
{
    /// <summary>
    ///     pixiv への API アクセスを行います。
    /// </summary>
    public class PixivApiClient : IPixivClient
    {
        private static Func<Expression<Func<string, object>>, string> F1 => expr => expr.Parameters[0].Name;

        private static Func<Expression<Func<string, object>>, string> F2
            => expr => expr.Compile().Invoke(null).ToString();

        public string AccessToken { get; internal set; }

        private IList<KeyValuePair<string, string>> GetPrameter(params Expression<Func<string, object>>[] parameters)
            => parameters.Select(w => new KeyValuePair<string, string>(F1(w), F2(w))).ToList();

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
        public IUserApi User => new UserApi(this);
        public IllustV2 IllustV2 => new IllustApiV2(this);
        public INovelV2 NovelV2 => new NovelApiV2(this);
        public IPximgApi Pximg => new PximgApi();

        private readonly HttpClient _httpClient;

        public PixivApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("App-Version", "6.0.1");
            _httpClient.DefaultRequestHeaders.Add("App-OS", "ios");
            _httpClient.DefaultRequestHeaders.Add("App-OS-Version", "9.3.2");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "PixivIOSApp/6.0.1 (iOS 9.3.2; iPhone7,2)");
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
                // Modified DefaultRequestheaders.
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        #endregion Implementation of IPixivClient
    }
}