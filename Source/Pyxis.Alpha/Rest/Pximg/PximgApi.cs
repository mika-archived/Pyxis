using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Pyxis.Alpha.Internal;
using Pyxis.Beta.Interfaces.Rest.Pximg;

namespace Pyxis.Alpha.Rest.Pximg
{
    public class PximgApi : IPximgApi
    {
        private readonly PixivApiClient _client;

        public PximgApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IPximgApi

        public async Task<Stream> GetAsync(string url)
        {
            // Debug.WriteLine("GETI :" + url);
            var client = new HttpClient(new PximgHttpClientHandler(_client));
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion
    }
}