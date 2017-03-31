using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        public ReactiveProperty<int> MaxHeight { get; }
        public ReactiveProperty<int> MaxWidth { get; }

        public IllustViewerPageViewModel()
        {
            ScrollBarVisibility = ScrollBarVisibility.Disabled;
            _postDetail = new PixivPostDetail<Illust>(null);
            var connector = _postDetail.ObserveProperty(w => w.Post).Where(w => w != null).Publish();
            OriginaiImageUrl = connector.Select(w => new Uri(w.MetaSinglePage.OriginalImageUrl ?? w.MetaPages.First().ImageUrls.Original))
                                        .ToReadOnlyReactiveProperty()
                                        .AddTo(this);
            MaxHeight = connector.Select(w => w.Height).ToReactiveProperty().AddTo(this);
            MaxWidth = connector.Select(w => w.Width).ToReactiveProperty().AddTo(this);
            connector.Delay(TimeSpan.FromMilliseconds(500)).ObserveOnUIDispatcher()
                     .Do(w => ScrollBarVisibility = ScrollBarVisibility.Auto).Subscribe().AddTo(this);
            connector.Connect().AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<IllustParameter>();
            _postDetail.ApplyForce(parameter.Illust);
        }

        public void OnSizeChanged(object _, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != default(Size)) return;
            MaxWidth.Value = (int) e.NewSize.Width;
            MaxHeight.Value = (int) e.NewSize.Height;
        }

        #region ScrollBarVisibility

        private ScrollBarVisibility _scrollBarVisibility;

        public ScrollBarVisibility ScrollBarVisibility
        {
            get { return _scrollBarVisibility; }
            set { SetProperty(ref _scrollBarVisibility, value); }
        }

        #endregion
    }
}