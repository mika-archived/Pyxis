using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface INovelMarkerApi
    {
        Task<IVoidReturn> AddAsync(params Expression<Func<string, object>>[] parameters);
    }
}