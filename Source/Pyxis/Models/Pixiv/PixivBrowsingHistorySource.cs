using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivBrowsingHistorySource<T, TU> : PixivModel, IIncrementalSource<TU> where T : Post
    {
        private readonly Func<T, TU> _converter;
        private Cursorable<IllustCollection> _previousIllustCursor;
        private Cursorable<NovelCollection> _previousNovelCursor;

        public PixivBrowsingHistorySource(PixivClient pixivClient, Func<T, TU> converter = null) : base(pixivClient)
        {
            _converter = converter ?? (w => (TU) Activator.CreateInstance(typeof(TU), w));
        }

        Task<IEnumerable<TU>> IIncrementalSource<TU>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return typeof(T) != typeof(Novel) ? GetPagedIllustsAsync(pageIndex) : GetPagedNovelsAsync(pageIndex);
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private async Task<IEnumerable<TU>> GetPagedIllustsAsync(int pageIndex)
        {
            if (_previousIllustCursor != null)
                _previousIllustCursor = await EffectiveCallAsync($"IllustHistory-${pageIndex}", () => _previousIllustCursor.NextPageAsync());
            else
                _previousIllustCursor = await EffectiveCallAsync($"IllustHistory-${pageIndex}", () => PixivClient.User.BrowsingHistory.IllustsAsync());
            return ((IllustCollection) _previousIllustCursor)?.Illusts.Cast<T>().Select(w => _converter.Invoke(w));
        }

        private async Task<IEnumerable<TU>> GetPagedNovelsAsync(int pageIndex)
        {
            if (_previousNovelCursor != null)
                _previousNovelCursor = await EffectiveCallAsync($"NovelHistory-${pageIndex}", () => _previousNovelCursor.NextPageAsync());
            else
                _previousNovelCursor = await EffectiveCallAsync($"NovelHistory-${pageIndex}", () => PixivClient.User.BrowsingHistory.NovelsAsync());
            return ((NovelCollection) _previousNovelCursor)?.Novels.Cast<T>().Select(w => _converter.Invoke(w));
        }
    }
}