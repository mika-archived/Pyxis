using System;

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
    public class PixivImageViewModel : TappableThumbnailViewModel
    {
        private readonly IIllust _illust;
        private readonly INavigationService _navigationService;

        public PixivImageViewModel(IIllust illust, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _illust = illust;
            _navigationService = navigationService;

            Thumbnailable = new PixivImage(illust, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            var parameter = new IllustDetailParameter {Illust = _illust};
            _navigationService.Navigate(_illust.PageCount == 1 ? "Detail.IllustDetail" : "Detail.MangaDetail",
                                        parameter.ToJson());
        }

        #endregion
    }
}