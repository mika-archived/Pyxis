using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Rest.v1
{
    public class ApplicationInfoApi : IApplicationInfoApi
    {
        private readonly PixivApiClient _client;

        public ApplicationInfoApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IApplicationInfoApi

        public async Task<IApplicationInfo> IosAsync()
        {
            var appInfo = await _client.GetAsync<ApplicationInfoOwner>(Endpoints.ApplicationInfoIos, false);
            return appInfo.ApplicationInfo;
        }

        #endregion
    }

    public class ApplicationInfoOwner
    {
        [JsonProperty("application_info")]
        public ApplicationInfo ApplicationInfo { set; get; }
    }
}