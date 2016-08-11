using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pyxis.Alpha.Internal
{
    public class PximgHttpClientHandler : PixivHttpClientHandler
    {
        public PximgHttpClientHandler(PixivApiClient client) : base(client)
        {

        }

        #region Overrides of PixivHttpClientHandler

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            request.Headers.Add("Referer", "https://app-api.pixiv.net/");
            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}