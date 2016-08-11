using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBookmarksApi : IUserBookmarksApi
    {
        private readonly PixivApiClient _client;

        public UserBookmarksApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBookmarksApi

        public async Task<IIllusts> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.UserBookmarksIllust, false, parameters);

        public async Task<INovels> NovelAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.UserBookmarksNovel, false, parameters);

        #endregion
    }
}