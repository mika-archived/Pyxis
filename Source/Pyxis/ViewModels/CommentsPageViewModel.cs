using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Pyxis.Helpers;
using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta;
using Sagitta.Models;

using CommentSource = Microsoft.Toolkit.Uwp.IncrementalLoadingCollection
    <Pyxis.Models.Pixiv.PixivCommentSource<Pyxis.ViewModels.Contents.CommentViewModel>,
        Pyxis.ViewModels.Contents.CommentViewModel>;

namespace Pyxis.ViewModels
{
    public class CommentsPageViewModel : ViewModel
    {
        private readonly PixivCommentSource<CommentViewModel> _commentSource;
        private readonly PixivClient _pixivClient;
        private readonly PixivPostDetail<Post> _postDetail;
        public ReadOnlyReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveProperty<Uri> AuthorIconUrl { get; }
        public ReadOnlyReactiveProperty<string> AuthorName { get; }
        public CommentSource Comments { get; }

        public CommentsPageViewModel(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            _postDetail = new PixivPostDetail<Post>(null);
            _commentSource = new PixivCommentSource<CommentViewModel>(pixivClient);
            Comments = new CommentSource(_commentSource);
            var connector = _postDetail.ObserveProperty(w => w.Post).Where(w => w != null).Publish();
            AuthorIconUrl = connector.Select(w => new Uri(w.User.ProfileImageUrls.Medium)).ToReadOnlyReactiveProperty().AddTo(this);
            AuthorName = connector.Select(w => w.User.Name).ToReadOnlyReactiveProperty().AddTo(this);
            Title = connector.Select(w => $"「{w.Title}」のコメント").ToReadOnlyReactiveProperty().AddTo(this);
            connector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<PostParameter<Illust>>();
            if (parameter.HasObject)
                _postDetail.ApplyForce(parameter.Post);
            else
                throw new NotSupportedException();
            _commentSource.Apply(parameter.Post, w => new CommentViewModel(w));
            RunHelper.RunOnUI(async () => await Comments.RefreshAsync());
        }
    }
}