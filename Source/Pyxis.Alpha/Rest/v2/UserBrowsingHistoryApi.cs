using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
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