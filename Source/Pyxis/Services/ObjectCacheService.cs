using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Pyxis.Extensions;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ObjectCacheService : IObjectCacheService
    {
        private readonly Dictionary<string, object> _cacheObjects;
        private readonly Dictionary<string, DateTime> _cacheTimes;

        public ObjectCacheService()
        {
            _cacheTimes = new Dictionary<string, DateTime>();
            _cacheObjects = new Dictionary<string, object>();
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(1)).Subscribe(_ =>
            {
                if (_cacheTimes.Count > MaxSize)
                {
                    var count = _cacheTimes.Count;
                    for (var i = count; i < MaxSize; i++)
                    {
                        var key = _cacheTimes.First().Key;
                        _cacheTimes.Remove(key);
                        _cacheObjects.Remove(key);
                    }
                }
                _cacheTimes.Where(w => w.Value.AddMinutes(Expire.TotalMinutes * 2) < DateTime.Now).ToList().ForEach(w =>
                {
                    _cacheTimes.Remove(w.Key);
                    _cacheObjects.Remove(w.Key);
                });
            });
        }

        public TimeSpan Expire { get; set; } = TimeSpan.FromMinutes(5);
        public int MaxSize { get; set; } = byte.MaxValue;

        public async Task<T> EffectiveCallAsync<T>(string identifier, Func<Task<T>> action, TimeSpan? expire = null)
        {
            if (_cacheTimes.ContainsKey(identifier))
            {
                if (_cacheTimes[identifier].AddMinutes((expire ?? Expire).TotalMinutes) > DateTime.Now)
                    return await Task.FromResult((T) _cacheObjects[identifier]);
                _cacheTimes[identifier] = DateTime.Now;
                _cacheObjects[identifier] = await action.Invoke();
                return (T) _cacheObjects[identifier];
            }
            _cacheTimes.Add(identifier, DateTime.Now);
            return (T) _cacheObjects.AddAndReturn(identifier, await action.Invoke());
        }

        public T EffectiveCall<T>(string identifier, Func<T> action, TimeSpan? expire = null)
        {
            if (_cacheTimes.ContainsKey(identifier))
            {
                if (_cacheTimes[identifier].AddMinutes((expire ?? Expire).TotalMinutes) > DateTime.Now)
                    return (T) _cacheObjects[identifier];
                _cacheTimes[identifier] = DateTime.Now;
                _cacheObjects[identifier] = action.Invoke();
                return (T) _cacheObjects[identifier];
            }
            _cacheTimes.Add(identifier, DateTime.Now);
            return (T) _cacheObjects.AddAndReturn(identifier, action.Invoke());
        }
    }
}