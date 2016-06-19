using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivNovelViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INovel _novel;

        public string Title => _novel.Title;
        public string Caption => _novel.Caption;
        public string CreatedAt => _novel.CreateDate.ToString("g");
        public int BookmarkCount => _novel.TotalBookmarks;
        public int ViewCount => _novel.TotalView;

        public ReadOnlyReactiveProperty<string> ThumbnailPath { get; private set; }

        public PixivNovelViewModel(INovel novel, IImageStoreService imageStoreService,
                                   INavigationService navigationService)
        {
            _novel = novel;
            _navigationService = navigationService;

            var nov = new PixivNovel(novel, imageStoreService);
            ThumbnailPath = nov.ObserveProperty(w => w.ThumbnailPath).ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}