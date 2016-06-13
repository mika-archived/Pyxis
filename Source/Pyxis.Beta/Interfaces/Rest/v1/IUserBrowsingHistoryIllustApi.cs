using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserBrowsingHistoryIllustApi
    {
        Task AddAsync(params Expression<Func<string, object>>[] parameters);
    }
}