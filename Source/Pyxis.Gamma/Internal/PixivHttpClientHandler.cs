using System.Net.Http;

namespace Pyxis.Gamma.Internal
{
    internal class PixivHttpClientHandler : DelegatingHandler
    {
        public PixivHttpClientHandler() : base(new HttpClientHandler())
        {
            ((HttpClientHandler) InnerHandler).UseCookies = true;
        }
    }
}