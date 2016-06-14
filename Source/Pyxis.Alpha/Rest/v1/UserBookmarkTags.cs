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

        public async Task<IBookmark> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Bookmark>(Endpoints.UserBookmarkTagsIllust, true, parameters);

        #endregion
    }
}