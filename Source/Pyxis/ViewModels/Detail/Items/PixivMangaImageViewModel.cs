using System;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail.Items
{
    public class PixivMangaImageViewModel : ThumbnailableViewModel
    {
        public PixivMangaImageViewModel(IIllust illust, int index, IImageStoreService imageStoreService)
        {
            Thumbnailable = new PixivMangaImage(illust, index, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }
    }
}