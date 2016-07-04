using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailPageViewModel : ThumbnailableViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivDetail _pixivUser;
        private PixivUserImage _pixivUserImage;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveProperty<string> Username { get; private set; }
        public ReadOnlyReactiveProperty<string> ScreenName { get; private set; }
        public ReadOnlyReactiveProperty<string> Url { get; private set; }
        public ReadOnlyReactiveProperty<string> Gender { get; private set; }
        public ReadOnlyReactiveProperty<string> Region { get; private set; }
        public ReadOnlyReactiveProperty<string> Birthday { get; private set; }
        public ReadOnlyReactiveProperty<string> Job { get; private set; }
        public ReadOnlyReactiveProperty<string> Twitter { get; private set; }
        public ReadOnlyReactiveProperty<string> Description { get; private set; }
        public ReadOnlyReactiveProperty<string> Computer { get; private set; }
        public ReadOnlyReactiveProperty<string> Monitor { get; private set; }
        public ReadOnlyReactiveProperty<string> Software { get; private set; }
        public ReadOnlyReactiveProperty<string> Scanner { get; private set; }
        public ReadOnlyReactiveProperty<string> Tablet { get; private set; }
        public ReadOnlyReactiveProperty<string> Mouse { get; private set; }
        public ReadOnlyReactiveProperty<string> Printer { get; private set; }
        public ReadOnlyReactiveProperty<string> Desktop { get; private set; }
        public ReadOnlyReactiveProperty<string> Bgm { get; private set; }
        public ReadOnlyReactiveProperty<string> Table { get; private set; }
        public ReadOnlyReactiveProperty<string> Chair { get; private set; }
        public ReadOnlyReactiveProperty<string> Other { get; private set; }

        public UserDetailPageViewModel(IAccountService accountService, IImageStoreService imageStoreService,
                                       INavigationService navigationService, IPixivClient pixivClient)
        {
            _accountService = accountService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            ThumbnailPath = PyxisConstants.DummyIcon;
        }

        private void Initialie(UserDetailParameter parameter)
        {
            var id = string.IsNullOrWhiteSpace(parameter.Id) ? _accountService.LoggedInAccount.Id : parameter.Id;
            _pixivUser = new PixivDetail(id, SearchType.Users, _pixivClient);
            var observer = _pixivUser.ObserveProperty(w => w.UserDetail).Where(w => w != null).Publish();
            observer.Subscribe(w =>
            {
                Thumbnailable = new PixivUserImage(w.User, _imageStoreService);
                Thumbnailable.ObserveProperty(v => v.ThumbnailPath)
                             .Where(v => !string.IsNullOrWhiteSpace(v))
                             .ObserveOnUIDispatcher()
                             .Subscribe(v => ThumbnailPath = v)
                             .AddTo(this);
            });
            Username = observer.Select(w => w.User.Name).ToReadOnlyReactiveProperty().AddTo(this);
            ScreenName = observer.Select(w => $"@{w.User.AccountName}").ToReadOnlyReactiveProperty().AddTo(this);
            Url = observer.Select(w => w.Profile.Webpage).ToReadOnlyReactiveProperty().AddTo(this);
            Gender = observer.Select(w => w.Profile.Gender).ToReadOnlyReactiveProperty().AddTo(this);
            Region = observer.Select(w => w.Profile.Region).ToReadOnlyReactiveProperty().AddTo(this);
            Birthday = observer.Select(w => w.Profile.Birth).ToReadOnlyReactiveProperty().AddTo(this);
            Job = observer.Select(w => w.Profile.Job).ToReadOnlyReactiveProperty().AddTo(this);
            Twitter = observer.Select(w => w.Profile.TwitterAccountUrl).ToReadOnlyReactiveProperty().AddTo(this);
            Description = observer.Select(w => w.User.Comment).ToReadOnlyReactiveProperty().AddTo(this);
            Computer = observer.Select(w => w.Workspace.Pc).ToReadOnlyReactiveProperty().AddTo(this);
            Monitor = observer.Select(w => w.Workspace.Monitor).ToReadOnlyReactiveProperty().AddTo(this);
            Software = observer.Select(w => w.Workspace.Tool).ToReadOnlyReactiveProperty().AddTo(this);
            Scanner = observer.Select(w => w.Workspace.Scanner).ToReadOnlyReactiveProperty().AddTo(this);
            Tablet = observer.Select(w => w.Workspace.Tablet).ToReadOnlyReactiveProperty().AddTo(this);
            Mouse = observer.Select(w => w.Workspace.Monitor).ToReadOnlyReactiveProperty().AddTo(this);
            Printer = observer.Select(w => w.Workspace.Printer).ToReadOnlyReactiveProperty().AddTo(this);
            Desktop = observer.Select(w => w.Workspace.Desktop).ToReadOnlyReactiveProperty().AddTo(this);
            Bgm = observer.Select(w => w.Workspace.Music).ToReadOnlyReactiveProperty().AddTo(this);
            Table = observer.Select(w => w.Workspace.Desk).ToReadOnlyReactiveProperty().AddTo(this);
            Chair = observer.Select(w => w.Workspace.Chair).ToReadOnlyReactiveProperty().AddTo(this);
            Other = observer.Select(w => w.Workspace.Comment).ToReadOnlyReactiveProperty().AddTo(this);
            observer.Connect().AddTo(this);

            _pixivUser.Fetch();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<UserDetailParameter>((string) e?.Parameter);
            Initialie(parameter);
        }

        #endregion
    }
}