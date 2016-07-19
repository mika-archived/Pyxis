using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserFollowApi
    {
        Task<IVoidReturn> AddAsync(params Expression<Func<string, object>>[] parameters);

        Task<IVoidReturn> DeleteAsunc(params Expression<Func<string, object>>[] parameters);
    }
}