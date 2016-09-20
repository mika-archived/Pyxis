using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest.Pximg;

namespace Pyxis.Alpha.Rest.Pximg
{
    public class PximgApi : IPximgApi
    {
        private readonly HttpClient _httpClient;

        public PximgApi()
        {
            _httpClient = new HttpClient();
        }

        #region Implementation of IPximgApi

        public async Task<Stream> GetAsync(string url)
        {
            // Debug.WriteLine("GETI :" + url);
            // Modified DefaultRequestheaders.
            _httpClient.DefaultRequestHeaders.Referrer = new Uri("https://app-api.pixiv.net/");
            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion Implementation of IPximgApi
    }
}