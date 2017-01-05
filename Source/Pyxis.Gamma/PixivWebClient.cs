using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Beta.Events;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;
using Pyxis.Gamma.Internal;
using Pyxis.Gamma.Web.v1;

namespace Pyxis.Gamma
{
    public class PixivWebClient : IPixivClient
    {
        private static Func<Expression<Func<string, object>>, string> F1 => expr => expr.Parameters[0].Name;

        private static Func<Expression<Func<string, object>>, string> F2
            => expr => expr.Compile().Invoke(null).ToString();

        private IList<KeyValuePair<string, string>> GetPrameter(params Expression<Func<string, object>>[] parameters)
            => parameters.Select(w => new KeyValuePair<string, string>(F1(w), F2(w))).ToList();

        #region Implementation of IPixivClient

        public IAuthorizationApi Authorization { get; }
        public IApplicationInfoApi ApplicationInfo => new ApplicationInfoApi(this);
        public IIllustApi IllustV1 { get; }
        public IMangaApi Manga { get; }
        public INovelApi NovelV1 { get; }
        public INovelApi Novel { get; }
        public ISearchApi Search { get; }
        public ISpotlightApi Spotlight { get; }
        public ITrendingTagsApi TrendingTags { get; }
        public IUgoiraApi Ugoira { get; }
        public IUserApi User { get; }
        public Beta.Interfaces.Rest.v2.IIllustApi IllustV2 { get; }
        public Beta.Interfaces.Rest.v2.INovelApi NovelV2 { get; }
        public IPximgApi Pximg { get; }

        public async Task<T> GetAsync<T>(string url, bool requireAuth, params Expression<Func<string, object>>[] parameters)
        {
            var client = new HttpClient(new PixivHttpClientHandler());
            var param = string.Join("&", GetPrameter(parameters)
                                             .Where(w => !string.IsNullOrWhiteSpace(w.Value))
                                             .Select(w => $"{w.Key}={Uri.EscapeDataString(w.Value)}"));
            url += "?" + param;
            try
            {
                Debug.WriteLine($"GET  :{url}");
                var response = await client.GetAsync(url);
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
            var client = new HttpClient(new PixivHttpClientHandler());
            var param = GetPrameter(parameters).Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
            var content = new FormUrlEncodedContent(param);

            try
            {
                Debug.WriteLine($"POST :{url}");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        public event ReAuthenticateEventHandler OnReAuthenticate;

        #endregion
    }
}