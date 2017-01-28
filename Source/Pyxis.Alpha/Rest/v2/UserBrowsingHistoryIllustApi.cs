using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    public class UserBrowsingHistoryIllustApi : IUserBrowsingHistoryIllustApi
    {
        private readonly PixivApiClient _client;

        public UserBrowsingHistoryIllustApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IUserBrowsingHistoryIllustApi

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public async Task<IVoidReturn> AddAsync(params Expression<Func<string, object>>[] parameters) =>
            await _client.PostAsync<VoidReturn>(Endpoints.UserBrowsingHistoryIllustAdd, true, ParameterUtil.Merge("illust_ids", parameters));

        #endregion
    }
}