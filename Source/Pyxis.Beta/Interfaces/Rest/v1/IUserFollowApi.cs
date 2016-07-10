using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserFollowApi
    {
        Task AddAsync(params Expression<Func<string, object>>[] parameters);

        Task DeleteAsunc(params Expression<Func<string, object>>[] parameters);
    }
}