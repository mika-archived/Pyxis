using System;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Services.Interfaces;

namespace Pyxis.Controls
{
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    public sealed class PixivImage : Control
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(PixivImage), new PropertyMetadata(default(object), OnSourceChanged));

        public static readonly DependencyProperty StretchProperty
            = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PixivImage), new PropertyMetadata(Stretch.Uniform));

        private readonly IFileCacheStorage _cacheService;

        private Image _image;
        private bool _isInitialized;

        public Stretch PlaceholderStretch
        {
            get => (Stretch) GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public PixivImage()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            DefaultStyleKey = typeof(PixivImageEx);
            _cacheService = PrismUnityApplication.Current.Container.Resolve<IFileCacheStorage>();
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as PixivImage;
            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
                control?.SetSource(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            _image = (Image) GetTemplateChild("Image");
            _isInitialized = true;
            SetSource(Source);
            base.OnApplyTemplate();
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        private async void SetSource(object source)
        {
            if (!_isInitialized || source == null)
                return;
            var uri = source as Uri;
            if (uri == null || IsHttpUri(uri) && !uri.Host.EndsWith("pximg.net"))
                _image.Source = CreateImageSource(source.ToString());
            else
                await LoadPixivImageAsync(uri.ToString());
        }

        private async Task LoadPixivImageAsync(string imageUri)
        {
            if (await _cacheService.ExistFileAsync(imageUri))
                _image.Source = CreateImageSource(await _cacheService.LoadFileAsync(imageUri));
            else
                _image.Source = CreateImageSource(await _cacheService.SaveFileAsync(imageUri));
        }

        private BitmapImage CreateImageSource(string uri)
        {
            var bitmapImage = new BitmapImage(new Uri(uri));
            return bitmapImage;
        }
    }
}