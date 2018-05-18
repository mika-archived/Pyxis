using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pyxis.Controls
{
    /// <summary>
    ///     スライドショーコントロール
    /// </summary>
    [TemplateVisualState(Name = "Image1ToImage2", GroupName = "CommonState")]
    [TemplateVisualState(Name = "Image2ToImage1", GroupName = "CommonState")]
    [TemplatePart(Name = "Image1", Type = typeof(PixivImage))]
    [TemplatePart(Name = "Image2", Type = typeof(PixivImage))]
    public sealed class PixivSlideshow : Control
    {
        public static readonly DependencyProperty ImageCollectionProperty =
            DependencyProperty.Register(nameof(ImageCollection), typeof(IList<string>), typeof(Slideshow),
                                        new PropertyMetadata(default(IList<string>), OnItemCollectionChanged));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(IntervalProperty), typeof(double), typeof(Slideshow), new PropertyMetadata(default(double)));

        private int _counter;
        private byte _currentImage;
        private IDisposable _disposable;
        private PixivImage _image1;
        private PixivImage _image2;

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
            _currentImage = 0;
        }

        protected override void OnApplyTemplate()
        {
            _image1 = (PixivImage) GetTemplateChild("Image1");
            _image2 = (PixivImage) GetTemplateChild("Image2");
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
            _disposable = Observable.Timer(interval, interval).Subscribe(async w =>
            {
                if (_currentImage == 0)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _image1.Source = imageCollection[Next(imageCollection)];
                        VisualStateManager.GoToState(this, "Image1ToImage2", true);
                    });
                    _currentImage = 1;
                }
                else
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _image2.Source = imageCollection[Next(imageCollection)];
                        VisualStateManager.GoToState(this, "Image2ToImage1", true);
                    });
                    _currentImage = 0;
                }
            });
        }

        private void StopSlideshow()
        {
            _disposable?.Dispose();
        }

        private int Next(IList<string> imageCollection)
        {
            if (imageCollection.Count <= _counter + 1)
                _counter = 0;
            else
                _counter++;
            return _counter;
        }
    }
}