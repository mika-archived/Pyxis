using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Pyxis.Controls
{
    /// <summary>
    ///     スライドショーコントロール
    /// </summary>
    [TemplatePart(Name = "Image1", Type = typeof(PixivImage))]
    [TemplatePart(Name = "Image2", Type = typeof(PixivImage))]
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    public sealed class PixivSlideshow : Control
    {
        public static readonly DependencyProperty ImageCollectionProperty =
            DependencyProperty.Register(nameof(ImageCollection), typeof(IList<string>), typeof(PixivSlideshow),
                                        new PropertyMetadata(default(IList<string>), OnItemCollectionChanged));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(IntervalProperty), typeof(double), typeof(PixivSlideshow), new PropertyMetadata(default(double)));

        private int _counter;
        private IDisposable _disposable;

        private PixivImage _image1;
        private PixivImage _image2;
        private byte _processMode;
        private Grid _rootGrid;

        public IList<string> ImageCollection
        {
            get => (IList<string>) GetValue(ImageCollectionProperty);
            set => SetValue(ImageCollectionProperty, value);
        }

        public double Interval
        {
            get => (double) GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public PixivSlideshow()
        {
            _counter = -1;
            _processMode = 0;
        }

        protected override void OnApplyTemplate()
        {
            _image1 = (PixivImage) GetTemplateChild("Image1");
            _image2 = (PixivImage) GetTemplateChild("Image2");
            _rootGrid = (Grid) GetTemplateChild("RootGrid");
            base.OnApplyTemplate();
        }

        private static void OnItemCollectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PixivSlideshow;
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue))
                return;

            control?.StopSlideshow();
            control?.StartSlideshow(e.NewValue);
        }

        private void StartSlideshow(object source)
        {
            var imageCollection = (IList<string>) source;

            // First load
            _image1.Source = imageCollection[Next(imageCollection)];
            _image2.Source = imageCollection[Next(imageCollection)];

            var interval = TimeSpan.FromSeconds(Interval);
            _disposable = Observable.Timer(interval, interval / 3).ObserveOn(Dispatcher).Subscribe(w =>
            {
                if (_processMode == 0)
                {
                    (_rootGrid.Resources["ImageFadeOut"] as Storyboard)?.Begin();
                    _processMode = 1;
                }
                else if (_processMode == 1)
                {
                    _image1.Source = imageCollection[_counter];
                    _processMode = 2;
                }
                else
                {
                    _image2.Opacity = 0;
                    _image2.Source = imageCollection[Next(imageCollection)];
                    _processMode = 0;
                }
            });
        }

        private void StopSlideshow()
        {
            _disposable?.Dispose();
        }

        private int Next(ICollection<string> imageCollection)
        {
            if (imageCollection.Count <= _counter + 1)
                _counter = 0;
            else
                _counter++;
            return _counter;
        }
    }
}