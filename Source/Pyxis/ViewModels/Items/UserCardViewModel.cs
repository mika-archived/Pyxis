using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels.Items
{
    internal class UserCardViewModel : TappableThumbnailViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly PixivClient _pixivClient;
        private readonly User _user;
        private readonly UserPreview _userPreview;

        public string Username => _userPreview.User.Name;

        public UserCardViewModel(UserPreview userPreview, IImageStoreService imageStoreService,
                                 INavigationService navigationService, PixivClient pixivClient)
        {
            _userPreview = userPreview;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            IsFollowing = _userPreview.User.IsFollowed;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Thumbnailable = new PixivUserImage(userPreview.User, imageStoreService);
        }

        public UserCardViewModel(User user, IImageStoreService imageStoreService, INavigationService navigationService, PixivClient pixivClient)
        {
            _user = user;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            IsFollowing = _userPreview.User.IsFollowed;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Thumbnailable = new PixivUserImage(user, imageStoreService);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new DetailByIdParameter {Id = _userPreview?.User.Id.ToString() ?? _user.Id.ToString()};
            _navigationService.Navigate("Detail.UserDetail", parameter.ToJson());
        }

        #endregion

        #region FollowCommand

        private ICommand _followCommand;

        public ICommand FollowCommand => _followCommand ?? (_followCommand = new DelegateCommand(Follow));

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async void Follow()
        {
            var id = _userPreview?.User?.Id ?? _user.Id;
            if ((_userPreview?.User ?? _user).IsFollowed)
                await _pixivClient.User.Follow.DeleteAsync(id);
            else
                await _pixivClient.User.Follow.AddAsync(id);
            IsFollowing = !IsFollowing;
        }

        #endregion

        #region IsFollowing

        private bool _isFollowing;

        public bool IsFollowing
        {
            get { return _isFollowing; }
            set { SetProperty(ref _isFollowing, value); }
        }

        #endregion
    }
}