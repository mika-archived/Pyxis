using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivRecommendSource<T, TU> : PixivModel, IIncrementalSource<TU> where T : Post
    {
        private readonly Func<T, TU> _converter;
        private readonly IllustType? _illustType;
        private Cursorable<IllustCollection> _previousIllustCursor;
        private Cursorable<NovelCollection> _previousNovelCursor;

        // for Illust and Manga
        public PixivRecommendSource(PixivClient pixivClient, IllustType? illustType = null, Func<T, TU> converter = null) : base(pixivClient)
        {
            if (typeof(T) != typeof(Novel) && illustType == null)
                throw new NotSupportedException();
            _illustType = illustType;
            _converter = converter ?? (w => (TU) Activator.CreateInstance(typeof(TU), w));
        }

        Task<IEnumerable<TU>> IIncrementalSource<TU>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return _illustType != null ? GetPagedIllustsAsync(pageIndex) : GetPagedNovelsAsync(pageIndex);
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private async Task<IEnumerable<TU>> GetPagedIllustsAsync(int pageIndex)
        {
            if (_previousIllustCursor != null)
                _previousIllustCursor = await EffectiveCallAsync($"IllustRecommend-${pageIndex}", () => _previousIllustCursor.NextPageAsync());
            else if (_illustType.Value == IllustType.Illust)
                _previousIllustCursor = await EffectiveCallAsync($"IllustRecommend-${pageIndex}", () => PixivClient.Illust.RecommendedAsync(false));
            else
                _previousIllustCursor = await EffectiveCallAsync($"MangaRecommend-${pageIndex}", () => PixivClient.Manga.RecommendedAsync());
            return ((IllustCollection) _previousIllustCursor)?.Illusts.Cast<T>().Select(w => _converter.Invoke(w));
        }

        private async Task<IEnumerable<TU>> GetPagedNovelsAsync(int pageIndex)
        {
            if (_previousNovelCursor != null)
                _previousNovelCursor = await EffectiveCallAsync($"NovelRecommend-${pageIndex}", () => _previousNovelCursor.NextPageAsync());
            else
                _previousNovelCursor = await EffectiveCallAsync($"NovelRecommend-${pageIndex}", () => PixivClient.Novel.RecommendedAsync(false));
            return ((NovelCollection) _previousNovelCursor)?.Novels.Cast<T>().Select(w => _converter.Invoke(w));
        }
    }
}