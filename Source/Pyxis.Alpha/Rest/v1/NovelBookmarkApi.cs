using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    internal class NovelBookmarkApi : INovelBookmarkApi
    {
        private readonly PixivApiClient _client;

        public NovelBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelBookmarkApi

        public async Task AddAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<Task>(Endpoints.NovelBookmarkAdd, true, parameters);

        #endregion
    }
}