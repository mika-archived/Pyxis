using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class NovelBookmarkApi : INovelBookmarkApi
    {
        private readonly PixivApiClient _client;

        public NovelBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelBookmarkApi

        public async Task<IVoidReturn> AddAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<VoidReturn>(Endpoints.NovelBookmarkAdd, true, parameters);

        public async Task<IVoidReturn> DeleteAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<VoidReturn>(Endpoints.NovelBookmarkDelete, true, parameters);

        #endregion
    }
}