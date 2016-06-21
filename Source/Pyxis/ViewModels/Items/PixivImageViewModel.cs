using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivImageViewModel : ThumbnailableViewModel
    {
        private readonly IIllust _illust;
        private readonly INavigationService _navigationService;

        public string Title => _illust.Title;
        public string Caption => _illust.Caption.Replace("<br />", Environment.NewLine);
        public string CreatedAt => _illust.CreateDate.ToString("g");
        public int BookmarkCount => _illust.TotalBookmarks;
        public int ViewCount => _illust.TotalView;
        public ReadOnlyCollection<string> Tools => _illust.Tools.ToList().AsReadOnly();
        public ReactiveCommand ItemTappedCommand { get; }

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
            ItemTappedCommand = new ReactiveCommand();
            ItemTappedCommand.Subscribe(w => Debug.WriteLine(w)).AddTo(this);
        }
    }
}