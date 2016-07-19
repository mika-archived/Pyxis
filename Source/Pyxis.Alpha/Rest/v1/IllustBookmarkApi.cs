﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Alpha.Rest.v1
{
    public class IllustBookmarkApi : IIllustBookmarkApi
    {
        private readonly PixivApiClient _client;

        public IllustBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IIllustBookmarkApi

        public async Task<IUsers> UsersAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.GetAsync<Users>(Endpoints.IllustBookmarkUsers, false, parameters);

        public async Task<IVoidReturn> AddAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<VoidReturn>(Endpoints.IllustBookmarkAdd, true, parameters);

        public async Task<IVoidReturn> DeleteAsync(params Expression<Func<string, object>>[] parameters)
            => await _client.PostAsync<VoidReturn>(Endpoints.IllustBookmarkDelete, true, parameters);

        #endregion
    }
}