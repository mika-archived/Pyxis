using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v2
{
    public interface IUserApi
    {
        IUserBrowsingHistoryApi BrowsingHistory { get; }

        Task<IUsers> ListAsync(params Expression<Func<string, object>>[] parameters);
    }
}