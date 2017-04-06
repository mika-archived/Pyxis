using System;
using System.Reactive.Linq;

using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta.Models;

namespace Pyxis.ViewModels.Partials
{
    public class CommentsAreaViewModel : ViewModel
    {
        public ReadOnlyReactiveProperty<bool> HasComments { get; }
        public ReadOnlyReactiveProperty<bool> HasMoreComments { get; }
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }
        public ReactiveCommand SeeMoreCommentCommand { get; }

        public CommentsAreaViewModel(Post post, PixivComment pixivComment)
        {
            Comments = pixivComment.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
            HasComments = pixivComment.ObserveProperty(w => w.TotalComments).Select(w => w > 0).ToReadOnlyReactiveProperty().AddTo(this);
            HasMoreComments = pixivComment.ObserveProperty(w => w.TotalComments).Select(w => w > 5).ToReadOnlyReactiveProperty().AddTo(this);
            SeeMoreCommentCommand = new ReactiveCommand();
            SeeMoreCommentCommand.Subscribe(w => NavigateTo("Comments", new PostParameter<Post> {Post = post})).AddTo(this);
        }
    }
}