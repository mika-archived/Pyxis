using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Sagitta;
using Sagitta.Models;

using WinRTXamlToolkit.Tools;

namespace Pyxis.Models.Pixiv
{
    internal class PixivComment : PixivModel
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
                commentCollection = await EffectiveCallAsync($"IllustComment-{post.Id}", () => PixivClient.Illust.CommentAsync(post.Id));
            else
                commentCollection = await EffectiveCallAsync($"NovelComment-{post.Id}", () => PixivClient.Novel.CommentsAsync(post.Id));
            Comments.Clear();
            commentCollection.Comments.Take(Pickup).ForEach(w => Comments.Add(w));
        }
    }
}