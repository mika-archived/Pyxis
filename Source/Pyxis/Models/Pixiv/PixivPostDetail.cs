using System.Threading.Tasks;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    internal class PixivPostDetail<T> : PixivModel where T : Post
    {
        public PixivPostDetail(PixivClient pixivClient) : base(pixivClient) { }

        public async Task FetchAsync(int postId)
        {
            if (typeof(T) == typeof(Illust))
                Post = await PixivClient.Illust.DetailAsync(postId) as T;
            else
                Post = await PixivClient.Novel.DetailAsync(postId) as T;
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