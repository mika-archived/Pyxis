using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private PixivDetail _pixivUser;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveProperty<string> Username { get; private set; }
        public ReadOnlyReactiveProperty<string> ScreenName { get; private set; }
        public ReadOnlyReactiveProperty<string> Url { get; private set; }
        public ReadOnlyReactiveProperty<Uri> NavigateUrl { get; private set; }
        public ReadOnlyReactiveProperty<string> Gender { get; private set; }
        public ReadOnlyReactiveProperty<string> Region { get; private set; }
        public ReadOnlyReactiveProperty<string> Birthday { get; private set; }
        public ReadOnlyReactiveProperty<string> Job { get; private set; }
        public ReadOnlyReactiveProperty<string> Twitter { get; private set; }
        public ReadOnlyReactiveProperty<Uri> TwitterUrl { get; private set; }
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

        public UserDetailPageViewModel(IAccountService accountService, ICategoryService categoryService,
                                       IImageStoreService imageStoreService, INavigationService navigationService,
                                       IPixivClient pixivClient)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            NavigationService = navigationService;
            _pixivClient = pixivClient;
            ThumbnailPath = PyxisConstants.DummyIcon;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<DetailByIdParameter>((string) e?.Parameter);
            Initialie(parameter);
        }

        #endregion

        #region Initializers

        private void Initialie(DetailByIdParameter parameter)
        {
            _categoryService.UpdateCategory();
            var id = string.IsNullOrWhiteSpace(parameter.Id) ? _accountService.LoggedInAccount.Id : parameter.Id;
            _pixivUser = new PixivDetail(id, SearchType.Users, _pixivClient);
            var observer = _pixivUser.ObserveProperty(w => w.UserDetail).Where(w => w != null).Publish();
            observer.ObserveOnUIDispatcher().Subscribe(w =>
            {
                Thumbnailable = new PixivUserImage(w.User, _imageStoreService);
                var param1 = new UserDetailParameter
                {
                    Detail = w,
                    ProfileType = ProfileType.Work,
                    ContentType = ContentType.Illust
                };
                Parameter = ParamGen.Generate(param1, v => v.ProfileType).Skip(1).ToList();
            });
            Username = observer.Select(w => w.User.Name).ToReadOnlyReactiveProperty().AddTo(this);
            ScreenName = observer.Select(w => $"@{w.User.AccountName}").ToReadOnlyReactiveProperty().AddTo(this);
            Url = observer.Select(w => w.Profile.Webpage).ToReadOnlyReactiveProperty().AddTo(this);
            NavigateUrl = observer.Select(w => w.Profile.Webpage)
                                  .Where(w => !string.IsNullOrWhiteSpace(w))
                                  .Select(w => new Uri(w))
                                  .ToReadOnlyReactiveProperty()
                                  .AddTo(this);
            Gender = observer.Select(w => w.Profile.Gender).ToReadOnlyReactiveProperty().AddTo(this);
            Region = observer.Select(w => w.Profile.Region).ToReadOnlyReactiveProperty().AddTo(this);
            Birthday = observer.Select(w => w.Profile.Birth).ToReadOnlyReactiveProperty().AddTo(this);
            Job = observer.Select(w => w.Profile.Job).ToReadOnlyReactiveProperty().AddTo(this);
            Twitter = observer.Select(w => w.Profile.TwitterAccount).ToReadOnlyReactiveProperty().AddTo(this);
            TwitterUrl = observer.Select(w => w.Profile.TwitterAccount)
                                 .Where(w => !string.IsNullOrWhiteSpace(w))
                                 .Select(w => new Uri($"https://twitter.com/{w}"))
                                 .ToReadOnlyReactiveProperty()
                                 .AddTo(this);
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

        #endregion

        #region Parameter

        private List<string> _parameter;

        public List<string> Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        #endregion
    }
}