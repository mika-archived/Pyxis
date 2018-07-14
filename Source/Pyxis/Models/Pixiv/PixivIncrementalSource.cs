using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Collections;

using Pyxis.Services.Interfaces;

using Sagitta;

namespace Pyxis.Models.Pixiv
{
    public abstract class PixivIncrementalSource<T, TU> : PixivModel, IIncrementalSource<TU>
    {
        protected IObjectCacheStorage ObjectCacheStorage { get; }
        protected Func<T, TU> Converter { get; }

        protected PixivIncrementalSource(PixivClient pixivClient, IObjectCacheStorage objectCacheStorage, Func<T, TU> converter = null)
            : base(pixivClient, objectCacheStorage)
        {
            ObjectCacheStorage = objectCacheStorage;
            Converter = converter ?? (w => (TU) Activator.CreateInstance(typeof(TU), w));
        }

        public abstract Task<IEnumerable<TU>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken());
    }
}