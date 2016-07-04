using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Items;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Search.Items
{
    internal class UserCardViewModel : TappableThumbnailViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IUserPreview _userPreview;

        public string Username => _userPreview.User.Name;

        public ObservableCollection<PixivThumbnailViewModel> UserWorks { get; }

        public UserCardViewModel(IUserPreview userPreview, IImageStoreService imageStoreService,
                                 INavigationService navigationService)
        {
            _userPreview = userPreview;
            _imageStoreService = imageStoreService;
            _navigationService = navigationService;
            UserWorks = new ObservableCollection<PixivThumbnailViewModel>();
            for (var i = 0; i < 3; i++)
                UserWorks.Add(i < _userPreview.Illusts.Count
                    ? CreateThumbnail(userPreview.Illusts[i])
                    : new PixivThumbnailViewModel());
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
            var parameter = new DetailByIdParameter {Id = _userPreview.User.Id};
            _navigationService.Navigate("Detail.UserDetail", parameter.ToJson());
        }

        #endregion

        private PixivThumbnailViewModel CreateThumbnail(IIllust illust)
            => new PixivThumbnailViewModel(illust, _imageStoreService, _navigationService);
    }
}