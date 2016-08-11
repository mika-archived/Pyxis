using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v2;

namespace Pyxis.Beta.Interfaces.Rest.v2
{
    public interface INovelBookmarkApi
    {
        Task<IBookmarkDetail> DetailAsync(params Expression<Func<string, object>>[] parameters);
    }
}