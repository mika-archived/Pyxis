﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class IllustRecommendSource<T> : PixivIncrementalSource<Illust, T>
    {
        private readonly List<Illust> _items;
        private IllustCollection _illustCollection;

        public IllustRecommendSource(PixivClient pixivClient, IObjectCacheStorage objectCacheStorage, Func<Illust, T> converter = null) : base(pixivClient, objectCacheStorage, converter)
        {
            _items = new List<Illust>();
        }

        public override async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            var items = pageIndex * pageSize;
            if (_items.Count > items)
                return _items.Skip(items).Take(pageSize).Select(w => Converter.Invoke(w));

            if (_illustCollection != null)
                _illustCollection = await CacheInvokeAsync(_illustCollection.NextUrl, async () => await _illustCollection.NextAsync());
            else
                _illustCollection = await CacheInvokeAsync("IllustRecommendSource", async () => await PixivClient.Illust.RecommendedAsync());

            _illustCollection.Illusts.ForEach(w => _items.Add(w));
            return _items.Skip(items).Take(pageSize).Select(w => Converter.Invoke(w));
        }
    }
}