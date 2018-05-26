using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Pyxis.Models;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    /// <summary>
    ///     同一セッション間でのみ有効な、オブジェクトを格納可能なキャッシュストレージを提供します。
    /// </summary>
    internal class SessionObjectCacheStorage : IObjectCacheStorage
    {
        private readonly Dictionary<string, ObjectCache> _cacheStorage;

        public SessionObjectCacheStorage()
        {
            _cacheStorage = new Dictionary<string, ObjectCache>();
        }

        /// <summary>
        ///     Default expired_at is 10 minutes.
        /// </summary>
        public TimeSpan Expire { get; } = TimeSpan.FromMinutes(10);

        public async Task<T> InvokeAsync<T>(string cacheKey, Func<Task<T>> action, TimeSpan? expire = null) where T : class
        {
            if (_cacheStorage.ContainsKey(cacheKey))
            {
                var objectCahe = _cacheStorage[cacheKey];
                if (objectCahe.ExpiredAt <= DateTime.Now)
                    return objectCahe.Value as T;
            }
            var value = await action.Invoke();
            var expiredAt = DateTime.Now + (expire ?? Expire);
            _cacheStorage.Add(cacheKey, new ObjectCache {ExpiredAt = expiredAt, Value = value});

            return value;
        }

        public async Task<T> InvokeAsync<T>(Func<Task<T>> action, TimeSpan? expire = null) where T : class
        {
            var stack = new StackFrame();
            return await InvokeAsync(stack.GetMethod().Name, action, expire);
        }

        public Task ClearAsync()
        {
            _cacheStorage.Clear();

            return Task.CompletedTask;
        }

        public Task ClearAsync(string cacheKey)
        {
            _cacheStorage.Remove(cacheKey);

            return Task.CompletedTask;
        }
    }
}