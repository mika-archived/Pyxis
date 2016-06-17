using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    internal class PixivNovelViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;

        public ReadOnlyReactiveProperty<string> ThumbnailPath { get; private set; }

        public PixivNovelViewModel(INovel novel, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _novel = novel;
            _navigationService = navigationService;

            var nov = new PixivNovel(novel, imageStoreService);
            nov.ShowThumbnail();
            ThumbnailPath = nov.ObserveProperty(w => w.ThumbnailPath).ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}