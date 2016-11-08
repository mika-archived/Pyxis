using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IQueryCacheService
    {
        T Run<T>(Func<Expression<Func<string, object>>[], T> dispatcher, params Expression<Func<string, object>>[] query);

        Task<T> RunAsync<T>(Func<Expression<Func<string, object>>[], Task<T>> dispatcher, params Expression<Func<string, object>>[] query);

        void Clear();
    }
}