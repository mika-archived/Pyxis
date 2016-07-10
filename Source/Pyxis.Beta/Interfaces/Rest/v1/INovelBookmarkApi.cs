using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface INovelBookmarkApi
    {
        Task AddAsync(params Expression<Func<string, object>>[] parameters);

        Task DeleteAsync(params Expression<Func<string, object>>[] parameters);
    }
}