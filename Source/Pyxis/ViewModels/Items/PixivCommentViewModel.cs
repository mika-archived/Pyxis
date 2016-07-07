using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Items
{
    public class PixivCommentViewModel : ThumbnailableViewModel
    {
        private readonly IComment _comment;

        public string Comment => _comment.CommentBody;
        public string CreatedAt => _comment.Date.ToString("g");
        public string Name => _comment.User.Name;

        public PixivCommentViewModel(IComment comment, IImageStoreService imageStoreService)
        {
            _comment = comment;
            Thumbnailable = new PixivUserImage(comment.User, imageStoreService);
        }
    }
}