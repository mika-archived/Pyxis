using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivRecommendSource<T> : PixivModel, IIncrementalSource<T> where T : Post
    {
        private readonly IllustType? _illustType;
        private Cursorable<IllustCollection> _previousIllustCursor;
        private Cursorable<NovelCollection> _previousNovelCursor;

        // for Illust and Manga
        public PixivRecommendSource(PixivClient pixivClient, IllustType? illustType = null) : base(pixivClient)
        {
            if (typeof(T) != typeof(Novel) && illustType == null)
                throw new NotSupportedException();
            _illustType = illustType;
        }

        Task<IEnumerable<T>> IIncrementalSource<T>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return _illustType != null ? GetPagedIllustsAsync() : GetPagedNovelsAsync();
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private async Task<IEnumerable<T>> GetPagedIllustsAsync()
        {
            if (_previousIllustCursor != null)
                _previousIllustCursor = await _previousIllustCursor.NextPageAsync();
            else if (_illustType.Value == IllustType.Illust)
                _previousIllustCursor = await PixivClient.Illust.RecommendedAsync(false);
            else
                _previousIllustCursor = await PixivClient.Manga.RecommendedAsync();
            return ((IllustCollection) _previousIllustCursor)?.Illusts as IEnumerable<T>;
        }

        private async Task<IEnumerable<T>> GetPagedNovelsAsync()
        {
            if (_previousNovelCursor != null)
                _previousNovelCursor = await _previousNovelCursor.NextPageAsync();
            else
                _previousNovelCursor = await PixivClient.Novel.RecommendedAsync(false);
            return ((NovelCollection) _previousNovelCursor)?.Novels as IEnumerable<T>;
        }
    }
}