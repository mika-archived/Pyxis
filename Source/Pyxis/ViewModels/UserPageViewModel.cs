using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Models.Enum;
using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta;

namespace Pyxis.ViewModels
{
    public class UserPageViewModel : ViewModel
    {
        private readonly PixivUser _pixivUser;
        private int _userId;

        public ReadOnlyReactiveProperty<string> Title { get; }
        public ReadOnlyReactiveProperty<string> Username { get; }
        public ReadOnlyReactiveProperty<string> ScreenName { get; }
        public ReadOnlyReactiveProperty<Uri> ProfileIcon { get; }
        public ReadOnlyReactiveProperty<Uri> ProfileBackground { get; }
        public ReadOnlyReactiveProperty<string> Description { get; }

        public UserPageViewModel(PixivClient pixivClient)
        {
            _pixivUser = new PixivUser(pixivClient);
            var connector = _pixivUser.ObserveProperty(w => w.UserDetail).Where(w => w != null).Publish();
            Title = connector.Select(w => $"ユーザー詳細 - {w.User.Name}").ToReadOnlyReactiveProperty("ユーザー詳細").AddTo(this);
            Username = connector.Select(w => w.User.Name).ToReadOnlyReactiveProperty().AddTo(this);
            ScreenName = connector.Select(w => $"@{w.User.Account}").ToReadOnlyReactiveProperty().AddTo(this);
            ProfileIcon = connector.Select(w => new Uri(w.User.ProfileImageUrls.Medium)).ToReadOnlyReactiveProperty().AddTo(this);
            ProfileBackground = connector.Select(w => string.IsNullOrWhiteSpace(w.Profile.BackgroundImageUrl)
                ? w.User.ProfileImageUrls.Medium
                : w.Profile.BackgroundImageUrl).Select(w => new Uri(w)).ToReadOnlyReactiveProperty().AddTo(this);
            Description = connector.Select(w => w.User.Comment).ToReadOnlyReactiveProperty().AddTo(this);
            Website = connector.Select(w => w.Profile.Website).ToReadOnlyReactiveProperty().AddTo(this);
            Gender = connector.Select(w => w.Profile.Gender?.ToString()).ToReadOnlyReactiveProperty().AddTo(this);
            Birthday = connector.Select(w => w.Profile.BirthDay).ToReadOnlyReactiveProperty().AddTo(this);
            Region = connector.Select(w => w.Profile.Region).ToReadOnlyReactiveProperty().AddTo(this);
            Job = connector.Select(w => w.Profile.Job).ToReadOnlyReactiveProperty().AddTo(this);
            TotalFollowings = connector.Select(w => w.Profile.TotalFollowUsers).ToReadOnlyReactiveProperty().AddTo(this);
            TotalFollowers = connector.Select(w => w.Profile.TotalFollower).ToReadOnlyReactiveProperty().AddTo(this);
            TotalMypixivs = connector.Select(w => w.Profile.TotalMypixivUsers).ToReadOnlyReactiveProperty().AddTo(this);
            TotalIllusts = connector.Select(w => w.Profile.TotalIllusts).ToReadOnlyReactiveProperty().AddTo(this);
            TotalMangas = connector.Select(w => w.Profile.TotalManga).ToReadOnlyReactiveProperty().AddTo(this);
            TotalNovels = connector.Select(w => w.Profile.TotalNovels).ToReadOnlyReactiveProperty().AddTo(this);
            TotalBookmarks = connector.Select(w => w.Profile.TotalIllustBookmarksPublic).ToReadOnlyReactiveProperty().AddTo(this);
            Twitter = connector.Select(w => $"@{w.Profile.TwitterAccount}").ToReadOnlyReactiveProperty().AddTo(this);
            Computer = connector.Select(w => w.Workspace.PC).ToReadOnlyReactiveProperty().AddTo(this);
            Monitor = connector.Select(w => w.Workspace.Monitor).ToReadOnlyReactiveProperty().AddTo(this);
            Tools = connector.Select(w => w.Workspace.Tool).ToReadOnlyReactiveProperty().AddTo(this);
            Scanner = connector.Select(w => w.Workspace.Scanner).ToReadOnlyReactiveProperty().AddTo(this);
            Tablet = connector.Select(w => w.Workspace.Tablet).ToReadOnlyReactiveProperty().AddTo(this);
            Mouse = connector.Select(w => w.Workspace.Monitor).ToReadOnlyReactiveProperty().AddTo(this);
            Printer = connector.Select(w => w.Workspace.Printer).ToReadOnlyReactiveProperty().AddTo(this);
            Desktop = connector.Select(w => w.Workspace.Desktop).ToReadOnlyReactiveProperty().AddTo(this);
            Music = connector.Select(w => w.Workspace.Music).ToReadOnlyReactiveProperty().AddTo(this);
            Desk = connector.Select(w => w.Workspace.Desk).ToReadOnlyReactiveProperty().AddTo(this);
            Chair = connector.Select(w => w.Workspace.Chair).ToReadOnlyReactiveProperty().AddTo(this);
            Comment = connector.Select(w => w.Workspace.Comment).ToReadOnlyReactiveProperty().AddTo(this);
            WorkspaceImage = connector.Select(w => w.Workspace.WorkspaceImageUrl)
                                      .Select(w => string.IsNullOrWhiteSpace(w) ? null : new Uri(w))
                                      .ToReadOnlyReactiveProperty().AddTo(this);
            connector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (e.Parameter != null)
            {
                var parameter = TransitionParameter.FromQuery<UserParameter>(e.Parameter.ToString());
                _userId = parameter.UserId;
            }
            else if (AccountService.Account != null)
            {
                _userId = int.Parse(AccountService.Account.Id);
            }
            else
            {
                // Not provide UserID and loggied in.
                RedirectTo("Login", new TransitionParameter {Mode = TransitionMode.Redirect});
            }
            RunHelper.RunAsync(async () =>
            {
                await _pixivUser.FetchAsync(_userId);
                return Unit.Default;
            });
        }

        #region Profile

        public ReadOnlyReactiveProperty<string> Website { get; }
        public ReadOnlyReactiveProperty<string> Gender { get; }
        public ReadOnlyReactiveProperty<string> Birthday { get; }
        public ReadOnlyReactiveProperty<string> Region { get; }
        public ReadOnlyReactiveProperty<string> Job { get; }
        public ReadOnlyReactiveProperty<int> TotalFollowings { get; }
        public ReadOnlyReactiveProperty<int> TotalFollowers { get; }
        public ReadOnlyReactiveProperty<int> TotalMypixivs { get; }
        public ReadOnlyReactiveProperty<int> TotalIllusts { get; }
        public ReadOnlyReactiveProperty<int> TotalMangas { get; }
        public ReadOnlyReactiveProperty<int> TotalNovels { get; }
        public ReadOnlyReactiveProperty<int> TotalBookmarks { get; }
        public ReadOnlyReactiveProperty<string> Twitter { get; }

        #endregion

        #region Workspace

        public ReadOnlyReactiveProperty<string> Computer { get; } // as 'PC'
        public ReadOnlyReactiveProperty<string> Monitor { get; }
        public ReadOnlyReactiveProperty<string> Tools { get; }
        public ReadOnlyReactiveProperty<string> Scanner { get; }
        public ReadOnlyReactiveProperty<string> Tablet { get; }
        public ReadOnlyReactiveProperty<string> Mouse { get; }
        public ReadOnlyReactiveProperty<string> Printer { get; }
        public ReadOnlyReactiveProperty<string> Desktop { get; }
        public ReadOnlyReactiveProperty<string> Music { get; }
        public ReadOnlyReactiveProperty<string> Desk { get; }
        public ReadOnlyReactiveProperty<string> Chair { get; }
        public ReadOnlyReactiveProperty<string> Comment { get; }
        public ReadOnlyReactiveProperty<Uri> WorkspaceImage { get; }

        #endregion
    }
}