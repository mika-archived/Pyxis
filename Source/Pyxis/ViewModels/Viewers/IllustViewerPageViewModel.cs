using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta.Models;

namespace Pyxis.ViewModels.Viewers
{
    public class IllustViewerPageViewModel : ViewModel
    {
        private readonly PixivPostDetail<Illust> _postDetail;
        public ReadOnlyReactiveProperty<Uri> OriginaiImageUrl { get; }
        public ReadOnlyReactiveProperty<int> MaxHeight { get; }
        public ReadOnlyReactiveProperty<int> MaxWidth { get; }

        public IllustViewerPageViewModel()
        {
            _postDetail = new PixivPostDetail<Illust>(null);
            var connector = _postDetail.ObserveProperty(w => w.Post).Where(w => w != null).Publish();
            OriginaiImageUrl = connector.Select(w => new Uri(w.MetaSinglePage.OriginalImageUrl ?? w.MetaPages.First().ImageUrls.Original))
                                        .ToReadOnlyReactiveProperty()
                                        .AddTo(this);
            MaxHeight = connector.Select(w => w.Height).ToReadOnlyReactiveProperty().AddTo(this);
            MaxWidth = connector.Select(w => w.Width).ToReadOnlyReactiveProperty().AddTo(this);
            connector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<IllustParameter>();
            _postDetail.ApplyForce(parameter.Illust);
        }
    }
}