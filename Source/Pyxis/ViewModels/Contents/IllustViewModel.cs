using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class IllustViewModel : ViewModel
    {
        private readonly Illust _illust;

        public string Title => _illust.Title;
        public string ThumbnailUrl => _illust.ImageUrls.Medium;
        public string Username => _illust.User.Name;
        public string UserIcon => _illust.User.ProfileImageUrls.Medium;

        public IllustViewModel(Illust illust)
        {
            _illust = illust;
        }
    }
}