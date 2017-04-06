using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Sagitta;
using Sagitta.Models;

using WinRTXamlToolkit.Tools;

namespace Pyxis.Models.Pixiv
{
    public class PixivComment : PixivModel
    {
        public ObservableCollection<Comment> Comments { get; }
        public int Pickup { get; set; } = 5;

        public PixivComment(PixivClient pixivClient) : base(pixivClient)
        {
            Comments = new ObservableCollection<Comment>();
        }

        public async Task FetchCommentAsync(Post post)
        {
            CommentCollection commentCollection;
            if (post is Illust)
                commentCollection = await EffectiveCallAsync($"IllustComment-{post.Id}_p0", () => PixivClient.Illust.CommentAsync(post.Id));
            else
                commentCollection = await EffectiveCallAsync($"NovelComment-{post.Id}_p0", () => PixivClient.Novel.CommentsAsync(post.Id));
            Comments.Clear();
            commentCollection.Comments.Take(Pickup).ForEach(w => Comments.Add(w));
            TotalComments = Comments.Any() ? commentCollection.TotalComments : 0;
        }

        #region TotalComments

        private int _totalComments;

        public int TotalComments
        {
            get => _totalComments;
            set => SetProperty(ref _totalComments, value);
        }

        #endregion
    }
}