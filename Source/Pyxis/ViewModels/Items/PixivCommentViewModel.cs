using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Items
{
    public class PixivCommentViewModel : ThumbnailableViewModel
    {
        private readonly Comment _comment;

        public string Comment => _comment.Body;
        public string CreatedAt => _comment.Date.ToString("g");
        public string Name => _comment.User.Name;

        public PixivCommentViewModel(Comment comment, IImageStoreService imageStoreService)
        {
            _comment = comment;
            Thumbnailable = new PixivUserImage(comment.User, imageStoreService);
        }
    }
}