using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Search.Items
{
    internal class UserCardViewModel : TappableThumbnailViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserPreview _userPreview;

        public string Username => _userPreview.User.Name;

        public UserCardViewModel(IUserPreview userPreview, IImageStoreService imageStoreService,
                                 INavigationService navigationService)
        {
            _userPreview = userPreview;
            _navigationService = navigationService;
            ThumbnailPath = PyxisConstants.DummyIcon;
            Thumbnailable = new PixivUserImage(userPreview.User, imageStoreService);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new DetailByIdParameter {Id = _userPreview.User.Id};
            _navigationService.Navigate("Detail.UserDetail", parameter.ToJson());
        }

        #endregion
    }
}