using System;
using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IObjectCacheStorage
    {
        TimeSpan Expire { get; }

        Task<T> InvokeAsync<T>(string cacheKey, Func<Task<T>> action, TimeSpan? expire = null) where T : class;

        Task<T> InvokeAsync<T>(Func<Task<T>> action, TimeSpan? expire = null) where T : class;

        Task ClearAsync();

        Task ClearAsync(string cacheKey);
    }
}