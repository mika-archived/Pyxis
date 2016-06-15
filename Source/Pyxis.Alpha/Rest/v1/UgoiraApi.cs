using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    internal class UgoiraApi : IUgoiraApi
    {
        private readonly PixivApiClient _client;

        public UgoiraApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUgoiraApi

        public async Task<IMetadata> MetadataAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Metadata>(Endpoints.UgoiraMetadata, false, parameters);

        // => await this._client.GetAsync<Metadata>()

        #endregion
    }
}