using System;

using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class CommentViewModel : ViewModel
    {
        private readonly Comment _comment;
        public string Body => _comment.Body;
        public string CreatedAt => _comment.Date.ToString("g");
        public string Username => _comment.User.Name;
        public Uri IconUri => new Uri(_comment.User.ProfileImageUrls.Medium);

        public CommentViewModel(Comment comment)
        {
            _comment = comment;
        }
    }
}