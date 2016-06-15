using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v2;
using Pyxis.Beta.Interfaces.Models.v2;
using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    internal class NovelBookmarkApi : INovelBookmarkApi
    {
        private readonly PixivApiClient _client;

        public NovelBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelBookmarkApi

        public async Task<IBookmarkDetail> DetailAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<BookmarkDetail>(Endpoints.NovelBookmarkDetail, true, parameters);

        #endregion
    }
}