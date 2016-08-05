using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pyxis.Alpha.Internal
{
    public class PixivHttpClientHandler : DelegatingHandler
    {
        private readonly PixivApiClient _client;

        public PixivHttpClientHandler(PixivApiClient client) : base(new HttpClientHandler())
        {
            _client = client;
        }

        #region Overrides of HttpClientHandler

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            ((HttpClientHandler) InnerHandler).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add("App-Version", "6.0.1");
            request.Headers.Add("App-OS", "ios");
            request.Headers.Add("App-OS-Version", "9.3.2");
            request.Headers.Add("User-Agent", "PixivIOSApp/6.0.1 (iOS 9.3.2; iPhone7,2)");
            if (!string.IsNullOrWhiteSpace(_client.AccessToken))
                request.Headers.Add("Authorization", $"Bearer {_client.AccessToken}");

            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}