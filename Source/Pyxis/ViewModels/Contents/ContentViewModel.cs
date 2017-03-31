using System;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public abstract class ContentViewModel : ViewModel
    {
        private readonly Post _post;

        public string Title => _post.Title;
        public string Username => $"by {_post.User.Name}";
        public string Tags => _post.Tags.Take(5).Select(w => w.Name).JoinStrings(", ");
        public abstract Uri Thumbnail { get; }

        protected ContentViewModel(Post post)
        {
            _post = post;
        }

        public abstract void NavigateTo();
    }
}