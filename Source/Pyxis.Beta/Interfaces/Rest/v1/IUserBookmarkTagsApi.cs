using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserBookmarkTagsApi
    {
        Task<IBookmarkTags> IllustAsync(params Expression<Func<string, object>>[] parameters);

        Task<IBookmarkTags> NovelAsync(params Expression<Func<string, object>>[] parameters);
    }
}