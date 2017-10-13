using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Pyxis.Models.Async;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivRelatedSource<T> : PixivModel, IIncrementalSource<T>
    {
        private readonly AsyncLock _asyncLock = new AsyncLock();
        private Func<Illust, T> _converter;
        private Illust _illust;
        private IllustCollection _previousCursor;

        public PixivRelatedSource(PixivClient pixivClient) : base(pixivClient)
        {
            Expire = TimeSpan.FromMinutes(30); // cache 30 minutes
        }

        async Task<IEnumerable<T>> IIncrementalSource<T>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            using (await _asyncLock.LockAsync())
            {
                if (_illust == null)
                    return new List<T>(); // Empty

                if (_previousCursor != null)
                    _previousCursor = await EffectiveCallAsync($"Related-{_illust.Id}_p{pageIndex}", () => _previousCursor.NextPageAsync());
                else
                    _previousCursor = await EffectiveCallAsync($"Related-{_illust.Id}_p0", () => PixivClient.Illust.RelatedAsync(_illust.Id));
                return _previousCursor?.Illusts.Select(w => _converter.Invoke(w));
            }
        }

        public void Apply(Illust illust, Func<Illust, T> converter)
        {
            _illust = illust;
            _converter = converter ?? (w => (T) Activator.CreateInstance(typeof(T), w));
        }
    }
}