using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBookmarkTags : IUserBookmarkTagsApi
    {
        private readonly PixivApiClient _client;

        public UserBookmarkTags(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBookmarkTagsApi

        public async Task<IBookmarkTags> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<BookmarkTags>(Endpoints.UserBookmarkTagsIllust, true, parameters);

        public async Task<IBookmarkTags> NovelAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<BookmarkTags>(Endpoints.UserBookmarkTagsNovel, true, parameters);

        #endregion
    }
}