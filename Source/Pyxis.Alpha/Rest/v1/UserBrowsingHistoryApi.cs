using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class UserBrowsingHistoryApi : IUserBrowsingHistoryApi
    {
        private readonly PixivApiClient _client;

        public UserBrowsingHistoryApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBrowsingHistoryApi

        public IUserBrowsingHistoryIllustApi Illust => new UserBrowsingHistoryIllustApi(_client);

        #endregion
    }
}