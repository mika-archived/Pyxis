using System;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivNovelViewModel : TappableThumbnailViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;

        public PixivNovelViewModel(INovel novel, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _novel = novel;
            _navigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivNovel(novel, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new NovelDetailParameter {Novel = _novel};
            _navigationService.Navigate("Detail.NovelDetail", parameter.ToJson());
        }

        #endregion
    }
}