using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    public class PixivImageViewModel : ViewModel
    {
        private readonly IIllust _illust;
        private readonly INavigationService _navigationService;

        public ReadOnlyReactiveProperty<string> Path { get; private set; }

        public PixivImageViewModel(IIllust illust, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _illust = illust;
            _navigationService = navigationService;

            var image = new PixivImage(illust, imageStoreService);
            image.Show();
            Path = image.ObserveProperty(w => w.ImagePath).ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}