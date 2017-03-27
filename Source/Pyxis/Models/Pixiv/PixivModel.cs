using System;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;

using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.Models.Pixiv
{
    public abstract class PixivModel : BindableBase
    {
        private readonly IObjectCacheService _objectCacheService;

        protected PixivClient PixivClient { get; }
        public TimeSpan? Expire { get; set; } = null;

        protected PixivModel(PixivClient pixivClient)
        {
            PixivClient = pixivClient;
            _objectCacheService = PrismUnityApplication.Current.Container.Resolve<IObjectCacheService>();
        }

        protected Task<T> EffectiveCallAsync<T>(string identifier, Func<Task<T>> action)
        {
            return _objectCacheService.EffectiveCallAsync(identifier, action, Expire);
        }

        protected T EffectiveCall<T>(string identifier, Func<T> action)
        {
            return _objectCacheService.EffectiveCall(identifier, action, Expire);
        }
    }
}