using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Pyxis.Models;
using Pyxis.Models.Caching;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class QueryCacheService : IQueryCacheService
    {
        private readonly AsyncLock _asyncLock = new AsyncLock();
        private readonly object _lockObj = new object();
        private readonly List<QueryCache> _queryCaches;

        public QueryCacheService()
        {
            _queryCaches = new List<QueryCache>();
        }

        public T Run<T>(Func<Expression<Func<string, object>>[], T> dispatcher, params Expression<Func<string, object>>[] query)
        {
            lock (_lockObj)
            {
                var builder = new QueryBuilder(dispatcher.GetMethodInfo().Name);
                builder.AddParams(query);
                var cache = _queryCaches.SingleOrDefault(w => w.IsEnabled && (w.Query == builder.ToQuery()));
                if (cache != null)
                    return (T) cache.Result;
                var result = dispatcher.Invoke(query);
                _queryCaches.Add(new QueryCache {Query = builder.ToQuery(), Result = result});
                return result;
            }
        }

        public async Task<T> RunAsync<T>(Func<Expression<Func<string, object>>[], Task<T>> dispatcher, params Expression<Func<string, object>>[] query)
        {
            using (await _asyncLock.LockAsync())
            {
                var builder = new QueryBuilder(dispatcher.GetMethodInfo().Name);
                builder.AddParams(query);
                var cache = _queryCaches.SingleOrDefault(w => w.IsEnabled && (w.Query == builder.ToQuery()));
                if (cache != null)
                    return (T) cache.Result;
                var result = await dispatcher.Invoke(query);
                _queryCaches.Add(new QueryCache {Query = builder.ToQuery(), Result = result});
                return result;
            }
        }

        public void Clear()
        {
            _queryCaches.Clear();
        }
    }
}