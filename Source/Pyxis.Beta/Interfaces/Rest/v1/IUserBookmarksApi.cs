﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserBookmarksApi
    {
        Task<IIllusts> IllustAsync(params Expression<Func<string, object>>[] parameters);

        Task<INovels> NovelAsync(params Expression<Func<string, object>>[] parameters);
    }
}