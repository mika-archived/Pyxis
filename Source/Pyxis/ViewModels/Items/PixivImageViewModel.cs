using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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

        public string Title => _illust.Title;
        public string Caption => _illust.Caption.Replace("<br />", Environment.NewLine);
        public string CreatedAt => _illust.CreateDate.ToString("g");
        public int BookmarkCount => _illust.TotalBookmarks;
        public int ViewCount => _illust.TotalView;
        public ReadOnlyCollection<string> Tools => _illust.Tools.ToList().AsReadOnly();

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
            if (_illust.PageCount == 1)
            {
                var parameter = new IllustDetailParameter {Illust = _illust};
                _navigationService.Navigate("Detail.IllustDetail", parameter.ToJson());
            }
            else
            {
                // マンガページ
                Debug.WriteLine("bbb");
            }
        }

        #endregion
    }
}