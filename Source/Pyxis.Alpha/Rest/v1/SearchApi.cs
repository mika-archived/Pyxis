using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class SearchApi : ISearchApi
    {
        private readonly PixivApiClient _client;

        public SearchApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of ISearchApi

        public async Task<IAutoComplete> AutoCompleteAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<AutoComplete>(Endpoints.SearchAutocomplete, false, parameters);

        public async Task<IIllusts> IllustAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Illusts>(Endpoints.SearchIllust, false, parameters);

        public async Task<INovels> NovelAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Novels>(Endpoints.SearchNovel, false, parameters);

        public async Task<IUserPreviews> UserAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<UserPreviews>(Endpoints.SearchUser, false, parameters);

        #endregion
    }
}