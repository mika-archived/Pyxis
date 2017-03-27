using System;

using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public abstract class ContentViewModel : ViewModel
    {
        private readonly Post _post;

        public string Title => _post.Title;
        public string Username => _post.User.Name;
        public abstract Uri Thumbnail { get; }

        protected ContentViewModel(Post post)
        {
            _post = post;
        }
    }
}