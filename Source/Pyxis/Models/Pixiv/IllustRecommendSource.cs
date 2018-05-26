using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class IllustRecommendSource<T> : PixivIncrementalSource<Illust, T>
    {
        private readonly IllustType _illustType;
        private readonly List<Illust> _items;
        private IllustCollection _illustCollection;

        public IllustRecommendSource(PixivClient pixivClient, IObjectCacheStorage objectCacheStorage, IllustType illustType, Func<Illust, T> converter = null)
            : base(pixivClient, objectCacheStorage, converter)
        {
            _illustType = illustType;
            _items = new List<Illust>();
        }

        public override async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize,
                                                                      CancellationToken cancellationToken = new CancellationToken())
        {
            var items = pageIndex * pageSize;
            if (_items.Count > items)
                return _items.Skip(items).Take(pageSize).Select(w => Converter.Invoke(w));

            if (_illustCollection != null)
            {
                _illustCollection = await _illustCollection.NextAsync();
            }
            else
            {
                if (_illustType == IllustType.Illust)
                    _illustCollection = await PixivClient.Illust.RecommendedAsync();
                else
                    _illustCollection = await PixivClient.Manga.RecommendedAsync();
            }

            _illustCollection.Illusts.ForEach(w => _items.Add(w));
            return _items.Skip(items).Take(pageSize).Select(w => Converter.Invoke(w));
        }
    }
}