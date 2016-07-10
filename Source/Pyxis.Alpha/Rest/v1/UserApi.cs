using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserApi : IUserApi
    {
        private readonly PixivApiClient _client;

        public UserApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserApi

        public IUserBookmarkTagsApi BookmarkTags => new UserBookmarkTags(_client);
        public IUserBookmarksApi Bookmarks => new UserBookmarksApi(_client);
        public IUserBrowsingHistoryApi BrowsingHistory => new UserBrowsingHistoryApi(_client);
        public IUserFollowApi Follow => new UserFollowApi(_client);

        public async Task<IUserDetail> DetailAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserDetail>(Endpoints.UserDetail, false, parameters);

        public async Task<IUserPreviews> FollowerAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.UserFollower, false, parameters);

        public async Task<IUserPreviews> FollowingAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.UserFollowing, false, parameters);

        public async Task<IIllusts> IllustsAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.UserIllusts, false, parameters);

        public async Task<IUsers> ListAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Users>(Endpoints.UserList, false, parameters);

        public async Task<IUserPreviews> MypixivAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.UserMypixiv, false, parameters);

        public async Task<INovels> NovelsAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.UserNovels, false, parameters);

        public async Task<IUserPreviews> RecommendedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.UserRecommended, false, parameters);

        public async Task<IUserPreviews> RelatedAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.UserRelated, true, parameters);

        #endregion
    }
}