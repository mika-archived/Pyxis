using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Pyxis.Controls
{
    /// <summary>
    ///     スライドショーコントロール
    /// </summary>
    [TemplateVisualState(Name = "Image1ToImage2", GroupName = "CommonState")]
    [TemplateVisualState(Name = "Image2ToImage1", GroupName = "CommonState")]
    [TemplatePart(Name = "Image1", Type = typeof(Image))]
    [TemplatePart(Name = "Image2", Type = typeof(Image))]
    public sealed class Slideshow : Control
    {
        public static readonly DependencyProperty ImageCollectionProperty =
            DependencyProperty.Register(nameof(ImageCollection), typeof(IList<string>), typeof(Slideshow),
                                        new PropertyMetadata(default(IList<string>), OnItemCollectionChanged));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(IntervalProperty), typeof(double), typeof(Slideshow), new PropertyMetadata(default(double)));

        private int _counter;
        private byte _currentImage;
        private IDisposable _disposable;
        private Image _image1;
        private Image _image2;

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

        public Slideshow()
        {
            _counter = 0;
            _currentImage = 0;
        }

        protected override void OnApplyTemplate()
        {
            _image1 = (Image) GetTemplateChild("Image1");
            _image2 = (Image) GetTemplateChild("Image2");
            base.OnApplyTemplate();
        }

        private static void OnItemCollectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Slideshow;
            if (e.OldValue.Equals(e.NewValue))
                return;

            control?.StopSlideshow();
            control?.StartSlideshow(e.NewValue);
        }

        private void StartSlideshow(object source)
        {
            _disposable = Observable.Timer(TimeSpan.FromSeconds(Interval)).Subscribe(w =>
            {
                if (_currentImage == 0)
                {
                    _image1.Source = new BitmapImage(new Uri(ImageCollection[Next()]));
                    _image2.Source = new BitmapImage(new Uri(ImageCollection[Next()]));
                    VisualStateManager.GoToState(this, "Image1ToImage2", true);
                    _currentImage = 1;
                }
                else
                {
                    _image1.Source = new BitmapImage(new Uri(ImageCollection[Next()]));
                    _image2.Source = new BitmapImage(new Uri(ImageCollection[Next()]));
                    VisualStateManager.GoToState(this, "Image2ToImage1", true);
                    _currentImage = 0;
                }
            });
        }

        private void StopSlideshow()
        {
            _disposable?.Dispose();
        }

        private int Next()
        {
            if (ImageCollection.Count <= _counter + 1)
                _counter = 0;
            else
                _counter++;
            return _counter;
        }
    }
}