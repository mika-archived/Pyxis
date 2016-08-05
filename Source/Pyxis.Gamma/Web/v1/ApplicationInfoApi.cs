using System;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Gamma.Web.v1
{
    internal class ApplicationInfoApi : IApplicationInfoApi
    {
        private readonly PixivWebClient _client;

        public ApplicationInfoApi(PixivWebClient client)
        {
            _client = client;
        }

        #region Implementation of IApplicationInfoApi

#pragma warning disable 1998

        public async Task<IApplicationInfo> IosAsync()
#pragma warning restore 1998
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}