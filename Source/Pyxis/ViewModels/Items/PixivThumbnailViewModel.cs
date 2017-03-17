using Prism.Windows.Navigation;

using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Sagitta.Models;

namespace Pyxis.ViewModels.Items
{
    public class PixivThumbnailViewModel : TappableThumbnailViewModel
    {
        protected INavigationService NavigationService { get; }
        protected Novel Novel { get; }
        protected Illust Illust { get; }

        /// <summary>
        ///     Constructor for blank image.
        /// </summary>
        public PixivThumbnailViewModel()
        {
            ThumbnailPath = PyxisConstants.DummyImage;
        }

        public PixivThumbnailViewModel(Illust illust, IImageStoreService imageStoreService,
                                       INavigationService navigationService)
        {
            Illust = illust;
            NavigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivImage(illust, imageStoreService);
            HasMultiple = illust.PageCount > 1;
        }

        public PixivThumbnailViewModel(Novel novel, IImageStoreService imageStoreService,
                                       INavigationService navigationService)
        {
            Novel = novel;
            NavigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivNovel(novel, imageStoreService);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            if (Illust != null)
            {
                var parameter = new IllustDetailParameter {Illust = Illust};
                NavigationService.Navigate("Detail.IllustDetail", parameter.ToJson());
            }
            else if (Novel != null)
            {
                var parameter = new NovelDetailParameter {Novel = Novel};
                NavigationService.Navigate("Detail.NovelDetail", parameter.ToJson());
            }
        }

        #endregion
    }
}