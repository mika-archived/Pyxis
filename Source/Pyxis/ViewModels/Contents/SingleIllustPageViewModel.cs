using System;
using System.Linq;
using System.Reactive.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Pyxis.Helpers;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class SingleIllustPageViewModel : ViewModel
    {
        public ReactiveCommand<SizeChangedEventArgs> OnSizeChangedCommand { get; }

        public SingleIllustPageViewModel(Illust illust, int page)
        {
            RunHelper.RunOnUI(() => ScrollBarVisibility = ScrollBarVisibility.Disabled);
            MaxHeight = illust.Height;
            MaxWidth = illust.Width;
            OriginalImageUrl = new Uri(illust.MetaSinglePage.OriginalImageUrl ?? illust.MetaPages.ToList()[page - 1].ImageUrls.Original);
            OnSizeChangedCommand = new ReactiveCommand<SizeChangedEventArgs>();
            OnSizeChangedCommand.Subscribe(w =>
            {
                if (w.PreviousSize != default(Size))
                    return;
                MaxHeight = w.NewSize.Height;
                MaxWidth = w.NewSize.Width;
            }).AddTo(this);

            Observable.Return(0).Delay(TimeSpan.FromMilliseconds(500))
                      .Subscribe(w => RunHelper.RunOnUI(() => ScrollBarVisibility = ScrollBarVisibility.Auto)).AddTo(this);
        }

        #region ScrollBarVisibility

        private ScrollBarVisibility _scrollBarVisibility;

        public ScrollBarVisibility ScrollBarVisibility
        {
            get { return _scrollBarVisibility; }
            set { SetProperty(ref _scrollBarVisibility, value); }
        }

        #endregion

        #region MaxHeight

        private double _maxHeight;

        public double MaxHeight
        {
            get { return _maxHeight; }
            set { SetProperty(ref _maxHeight, value); }
        }

        #endregion

        #region MaxWidth

        private double _maxWidth;

        public double MaxWidth
        {
            get { return _maxWidth; }
            set { SetProperty(ref _maxWidth, value); }
        }

        #endregion

        #region OriginalImageUrl

        private Uri _originalImageUrl;

        public Uri OriginalImageUrl
        {
            get { return _originalImageUrl; }
            set { SetProperty(ref _originalImageUrl, value); }
        }

        #endregion
    }
}