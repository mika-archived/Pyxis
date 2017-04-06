using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp;

using Pyxis.Extensions;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    public class PixivCommentSource<T> : PixivModel, IIncrementalSource<T>
    {
        private Func<Comment, T> _converter;
        private Post _post;
        private Cursorable<CommentCollection> _previousCommentCursor;

        public PixivCommentSource(PixivClient pixivClient) : base(pixivClient)
        {
            Expire = TimeSpan.FromMinutes(30); // cache 30 minutes
        }

        async Task<IEnumerable<T>> IIncrementalSource<T>.GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            if (_post == null)
                return new List<T>(); // Empty list

            if (_previousCommentCursor == null)
                if (_post is Illust)
                    await GetPagedIllustCommentsAsync();
                else
                    await GetPagedNovelCommentsAsync();
            else
                _previousCommentCursor =
                    await EffectiveCallAsync($"{_post.GetIdentifier()}Comment-{_post.Id}_p{pageIndex}", () => _previousCommentCursor.NextPageAsync());
            return ((CommentCollection) _previousCommentCursor)?.Comments.Select(w => _converter.Invoke(w));
        }

        public void Apply(Post post, Func<Comment, T> converter)
        {
            _post = post;
            _converter = converter;
        }

        private async Task GetPagedIllustCommentsAsync()
        {
            _previousCommentCursor = await EffectiveCallAsync($"IllustComment-{_post.Id}_p0", () => PixivClient.Illust.CommentAsync(_post.Id, false));
        }

        private async Task GetPagedNovelCommentsAsync()
        {
            _previousCommentCursor = await EffectiveCallAsync($"NovelComment-{_post.Id}_p0", () => PixivClient.Novel.CommentsAsync(_post.Id));
        }
    }
}