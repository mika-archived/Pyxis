using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Prism.Mvvm;

using Sagitta;

namespace Pyxis.Models.Pixiv
{
    public abstract class PixivModel : BindableBase
    {
        private readonly Dictionary<string, DateTime> _cacheTable;
        protected PixivClient PixivClient { get; }
        public TimeSpan Expire { get; set; } = TimeSpan.FromMinutes(5);

        protected PixivModel(PixivClient pixivClient)
        {
            PixivClient = pixivClient;
            _cacheTable = new Dictionary<string, DateTime>();
        }

        protected async Task<T> EffectiveCallAsync<T>(string identifier, Func<Task<T>> action, T ignore)
        {
            if (_cacheTable.ContainsKey(identifier))
            {
                if (_cacheTable[identifier].AddMinutes(Expire.TotalMinutes) > DateTime.Now)
                    return await Task.FromResult(ignore);
                _cacheTable[identifier] = DateTime.Now;
                return await action.Invoke();
            }
            _cacheTable.Add(identifier, DateTime.Now);
            return await action.Invoke();
        }

        protected T EffectiveCall<T>(string identifier, Func<T> action, T ignore)
        {
            if (_cacheTable.ContainsKey(identifier))
            {
                if (_cacheTable[identifier].AddMinutes(Expire.TotalMinutes) > DateTime.Now)
                    return ignore;
                _cacheTable[identifier] = DateTime.Now;
                return action.Invoke();
            }
            _cacheTable.Add(identifier, DateTime.Now);
            return action.Invoke();
        }
    }
}