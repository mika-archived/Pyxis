using System;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Microsoft.Practices.Unity;
using Microsoft.Toolkit.Uwp.UI.Controls;

using Prism.Unity.Windows;

using Pyxis.Services.Interfaces;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Pyxis.Controls
{
    /// <summary>
    ///     Pixiv の画像に
    /// </summary>
    [TemplatePart(Name = "Image", Type = typeof(ImageEx))]
    public sealed class PixivImageEx : Control
    {
        public static readonly DependencyProperty PlaceholderSourceProperty =
            DependencyProperty.Register(nameof(PlaceholderSource), typeof(ImageSource), typeof(PixivImageEx), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty PlaceholderStretchProperty =
            DependencyProperty.Register(nameof(PlaceholderStretch), typeof(Stretch), typeof(PixivImageEx), new PropertyMetadata(default(Stretch)));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(PixivImageEx), new PropertyMetadata(default(object), OnSourceChanged));

        public static readonly DependencyProperty NineGridProperty =
            DependencyProperty.Register(nameof(NineGrid), typeof(Thickness), typeof(PixivImageEx), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty StretchProperty
            = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PixivImageEx), new PropertyMetadata(Stretch.Uniform));

        private readonly IFileCacheStorage _cacheService;

        private ImageEx _image;
        private bool _isInitialized;

        public ImageSource PlaceholderSource
        {
            get => (ImageSource) GetValue(PlaceholderSourceProperty);
            set => SetValue(PlaceholderSourceProperty, value);
        }

        public Stretch PlaceholderStretch
        {
            get => (Stretch) GetValue(PlaceholderStretchProperty);
            set => SetValue(PlaceholderStretchProperty, value);
        }

        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Thickness NineGrid
        {
            get => (Thickness) GetValue(NineGridProperty);
            set => SetValue(NineGridProperty, value);
        }

        public Stretch Stretch
        {
            get => (Stretch) GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public PixivImageEx()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            DefaultStyleKey = typeof(PixivImageEx);
            _cacheService = PrismUnityApplication.Current.Container.Resolve<IFileCacheStorage>();
        }

        public event ImageExOpenedEventHandler ImageExOpened;

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as PixivImageEx;
            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
                control?.SetSource(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            _image = (ImageEx) GetTemplateChild("ImageEx");
            _image.ImageExOpened += (sender, e) => ImageExOpened?.Invoke(sender, e);
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
            if (!_isInitialized)
                return;
            var uri = source as Uri;
            if (uri == null || IsHttpUri(uri) && !uri.Host.EndsWith("pximg.net"))
                _image.Source = source;
            else
                await LoadPixivImageAsync(uri.ToString());
        }

        private async Task LoadPixivImageAsync(string imageUri)
        {
            if (await _cacheService.ExistFileAsync(imageUri))
                _image.Source = await _cacheService.LoadFileAsync(imageUri);
            else
                _image.Source = await _cacheService.SaveFileAsync(imageUri);
        }
    }
}