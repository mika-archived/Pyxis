using System;
using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IObjectCacheService
    {
        TimeSpan Expire { get; set; }
        int MaxSize { get; set; }

        Task<T> EffectiveCallAsync<T>(string identifier, Func<Task<T>> action, TimeSpan? expire = null, bool overwrite = false);

        T EffectiveCall<T>(string identifier, Func<T> action, TimeSpan? expire = null, bool overwrite = false);
    }
}