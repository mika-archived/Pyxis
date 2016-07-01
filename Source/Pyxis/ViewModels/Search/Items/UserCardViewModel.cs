using System;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Search.Items
{
    internal class UserCardViewModel : TappableThumbnailViewModel
    {
        private IImageStoreService _imageStoreService;
        private INavigationService _navigationService;
        private IUserPreview _userPreview;

        public UserCardViewModel(IUserPreview userPreview, IImageStoreService imageStoreService,
                                 INavigationService navigationService)
        {
            _userPreview = userPreview;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyIcon;
            Thumbnailable = new PixivUserImage(userPreview.User, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            base.OnItemTapped();
        }

        #endregion
    }
}