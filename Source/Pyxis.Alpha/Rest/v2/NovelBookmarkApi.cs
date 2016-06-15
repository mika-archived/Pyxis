using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Models.v2;
using Pyxis.Beta.Interfaces.Models.v2;
using Pyxis.Beta.Interfaces.Rest.v2;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Rest.v2
{
    public class NovelBookmarkApi : INovelBookmarkApi
    {
        private readonly PixivApiClient _client;

        public NovelBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelBookmarkApi

        public async Task<IBookmarkDetail> DetailAsync(params Expression<Func<string, object>>[] parameters)
        {
            var detail = await _client.GetAsync<BookmarkDetailOwner>(Endpoints.NovelBookmarkDetail, true, parameters);
            return detail.BookmarkDetail;
        }

        #endregion
    }

    public class BookmarkDetailOwner
    {
        [JsonProperty("bookmark_detail")]
        public BookmarkDetail BookmarkDetail { get; set; }
    }
}