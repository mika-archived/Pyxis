using System;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.Models.Pixiv
{
    public class PixivModel : BindableBase
    {
        protected PixivClient PixivClient { get; }
        protected IObjectCacheStorage CacheStorage { get; }

        protected PixivModel(PixivClient pixivClient, IObjectCacheStorage objectCacheStorage)
        {
            PixivClient = pixivClient;
            CacheStorage = objectCacheStorage;
        }

        protected async Task<T> CacheInvokeAsync<T>(string cacheKey, Func<Task<T>> action) where T : class
        {
            return await CacheStorage.InvokeAsync(cacheKey, action);
        }
    }
}