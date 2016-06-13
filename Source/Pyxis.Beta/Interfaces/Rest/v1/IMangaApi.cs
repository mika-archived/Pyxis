using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IMangaApi
    {
        Task<IIllusts> RecommendedAsync(params Expression<Func<string, object>>[] parameters);
    }
}