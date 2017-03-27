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
    internal class PixivRankingSource<T> : PixivModel, IIncrementalSource<T> where T : Post
    {
        private readonly DateTime? _date;
        private readonly RankingMode _mode;
        private Cursorable<IllustCollection> _previousIllustCursor;
        private Cursorable<NovelCollection> _previousNovelCursor;

        public PixivRankingSource(PixivClient pixivClient, RankingMode mode, DateTime? date) : base(pixivClient)
        {
            _mode = mode;
            _date = date;
        }

        Task<IEnumerable<T>> IIncrementalSource<T>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return typeof(T) == typeof(Illust) ? GetPagedIllustsAsync() : GetPagedNovelsAsync();
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private async Task<IEnumerable<T>> GetPagedIllustsAsync()
        {
            if (_previousIllustCursor != null)
                _previousIllustCursor = await _previousIllustCursor.NextPageAsync();
            else
                _previousIllustCursor = await PixivClient.Illust.RankingAsync(_mode, _date);
            return ((IllustCollection) _previousIllustCursor)?.Illusts as IEnumerable<T>;
        }

        private async Task<IEnumerable<T>> GetPagedNovelsAsync()
        {
            if (_previousNovelCursor != null)
                _previousNovelCursor = await _previousNovelCursor.NextPageAsync();
            else
                _previousNovelCursor = await PixivClient.Novel.RankingAsync(_mode, _date);
            return ((NovelCollection) _previousNovelCursor)?.Novels as IEnumerable<T>;
        }
    }
}