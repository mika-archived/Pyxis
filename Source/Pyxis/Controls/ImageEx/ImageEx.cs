using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Services.Interfaces;

using MsImageEx = Microsoft.Toolkit.Uwp.UI.Controls.ImageEx;

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    internal class ImageEx : Control
    {
        public static readonly DependencyProperty PlaceholderSourceProperty =
            DependencyProperty.Register(nameof(PlaceholderSource), typeof(ImageSource), typeof(ImageEx), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty PlaceholderStretchProperty =
            DependencyProperty.Register(nameof(PlaceholderStretch), typeof(Stretch), typeof(ImageEx), new PropertyMetadata(default(Stretch)));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(ImageEx), new PropertyMetadata(default(object), OnSourceChanged));

        public static readonly DependencyProperty NineGridProperty =
            DependencyProperty.Register(nameof(NineGrid), typeof(Thickness), typeof(ImageEx), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty StretchProperty
            = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));

        private readonly ICacheService _cacheService;

        private MsImageEx _image;
        private bool _isInitialized;
        private readonly string[] _targetHosts = {"pixiv", "pximg"};

        public ImageSource PlaceholderSource
        {
            get { return (ImageSource) GetValue(PlaceholderSourceProperty); }
            set { SetValue(PlaceholderSourceProperty, value); }
        }

        public Stretch PlaceholderStretch
        {
            get { return (Stretch) GetValue(PlaceholderStretchProperty); }
            set { SetValue(PlaceholderStretchProperty, value); }
        }

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public Thickness NineGrid
        {
            get { return (Thickness) GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public ImageEx()
        {
            DefaultStyleKey = typeof(ImageEx);
            _cacheService = PrismUnityApplication.Current.Container.Resolve<ICacheService>();
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as ImageEx;
            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
                control?.SetSource(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            _image = GetTemplateChild("Image") as MsImageEx;
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
            if (uri == null || IsHttpUri(uri) && !_targetHosts.Any(w => uri.Host.Contains(w)))
                _image.Source = source;
            else
                await LoadPixivImageAsync(uri.ToString());
        }

        private async Task LoadPixivImageAsync(string imageUri)
        {
            VisualStateManager.GoToState(_image, "Loading", true);
            if (await _cacheService.ExistFileAsync(imageUri))
                _image.Source = await _cacheService.LoadFileAsync(imageUri);
            else
                _image.Source = await _cacheService.SaveFileAsync(imageUri);
        }
    }
}