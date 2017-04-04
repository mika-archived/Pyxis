using System;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Pyxis.Helpers;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class SingleIllustPageViewModel : ViewModel
    {
        private readonly Illust _illust;
        public ReactiveCommand<SizeChangedEventArgs> OnScrollViewerSizeChangedCommand { get; }
        public ReactiveCommand<ImageExOpenedEventArgs> OnImageExOpenedCommand { get; }

        public SingleIllustPageViewModel(Illust illust, int page)
        {
            _illust = illust;
            RunHelper.RunOnUI(() => ScrollBarVisibility = ScrollBarVisibility.Disabled);
            MaxHeight = illust.Height;
            MaxWidth = illust.Width;
            OriginalImageUrl = new Uri(illust.MetaSinglePage.OriginalImageUrl ?? illust.MetaPages.ToList()[page - 1].ImageUrls.Original);
            OnScrollViewerSizeChangedCommand = new ReactiveCommand<SizeChangedEventArgs>();
            OnScrollViewerSizeChangedCommand.Subscribe(w => ApplyNewSize(w.NewSize)).AddTo(this);
            OnImageExOpenedCommand = new ReactiveCommand<ImageExOpenedEventArgs>();
            OnImageExOpenedCommand.Subscribe(w => ScrollBarVisibility = ScrollBarVisibility.Auto).AddTo(this);
        }

        private void ApplyNewSize(Size size)
        {
            MaxHeight = size.Height < _illust.Height ? size.Height : _illust.Height;
            MaxWidth = size.Width < _illust.Width ? size.Width : _illust.Width;
        }

        #region ScrollBarVisibility

        private ScrollBarVisibility _scrollBarVisibility;

        public ScrollBarVisibility ScrollBarVisibility
        {
            get => _scrollBarVisibility;
            set => SetProperty(ref _scrollBarVisibility, value);
        }

        #endregion

        #region MaxHeight

        private double _maxHeight;

        public double MaxHeight
        {
            get => _maxHeight;
            set => SetProperty(ref _maxHeight, value);
        }

        #endregion

        #region MaxWidth

        private double _maxWidth;

        public double MaxWidth
        {
            get => _maxWidth;
            set => SetProperty(ref _maxWidth, value);
        }

        #endregion

        #region OriginalImageUrl

        private Uri _originalImageUrl;

        public Uri OriginalImageUrl
        {
            get => _originalImageUrl;
            set => SetProperty(ref _originalImageUrl, value);
        }

        #endregion
    }
}