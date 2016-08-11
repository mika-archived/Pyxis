using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UgoiraApi : IUgoiraApi
    {
        private readonly PixivApiClient _client;

        public UgoiraApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUgoiraApi

        public async Task<IMetadata> MetadataAsync(params Expression<Func<string, object>>[] parameters)
        {
            var metadata = await _client.GetAsync<MetadataOwner>(Endpoints.UgoiraMetadata, false, parameters);
            return metadata.Metadata;
        }

        #endregion
    }

    public class MetadataOwner
    {
        [JsonProperty("ugoira_metadata")]
        public Metadata Metadata { get; set; }
    }
}