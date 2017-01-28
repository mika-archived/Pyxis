using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Items
{
    internal class UserCardViewModel : TappableThumbnailViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private readonly IUser _user;
        private readonly IUserPreview _userPreview;

        public string Username => _userPreview.User.Name;

        public UserCardViewModel(IUserPreview userPreview, IImageStoreService imageStoreService,
                                 INavigationService navigationService, IPixivClient pixivClient)
        {
            _userPreview = userPreview;
            _navigationService = navigationService;
            _pixivClient = pixivClient;
            IsFollowing = _userPreview.User.IsFollowed;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Thumbnailable = new PixivUserImage(userPreview.User, imageStoreService);
        }

        public UserCardViewModel(IUser user, IImageStoreService imageStoreService, INavigationService navigationService, IPixivClient pixivClient)
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
            var parameter = new DetailByIdParameter {Id = _userPreview?.User.Id ?? _user.Id};
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
                await _pixivClient.User.Follow.DeleteAsunc(user_id => id);
            else
                await _pixivClient.User.Follow.AddAsync(user_id => id);
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