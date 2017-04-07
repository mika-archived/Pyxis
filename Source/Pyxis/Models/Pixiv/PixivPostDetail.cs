using System;
using System.Threading.Tasks;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    internal class PixivPostDetail<T> : PixivModel where T : Post
    {
        public PixivPostDetail(PixivClient pixivClient) : base(pixivClient)
        {
            Expire = TimeSpan.FromHours(1); // cache 1 hour
        }

        public async Task FetchAsync(int postId)
        {
            if (typeof(T) == typeof(Illust))
                Post = (T) (Post) await EffectiveCallAsync($"IllustDetail-{postId}", () => PixivClient.Illust.DetailAsync(postId));
            else
                Post = (T) (Post) await EffectiveCallAsync($"NovelDetail-{postId}", () => PixivClient.Novel.DetailAsync(postId));
        }

        public void ApplyForce(T post)
        {
            Post = post;
        }

        #region Title

        private T _post;

        public T Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion
    }
}