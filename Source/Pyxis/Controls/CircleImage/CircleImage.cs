using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Services.Interfaces;

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    [TemplatePart(Name = "ImageBorder", Type = typeof(Border))]
    public class CircleImage : Control
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(CircleImage), new PropertyMetadata(default(object), OnSourceChanged));

        public static readonly DependencyProperty CircleBorderBrushProperty =
            DependencyProperty.Register(nameof(CircleBorderBrush), typeof(Brush), typeof(CircleImage), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CircleBorderThicknessProperty =
            DependencyProperty.Register(nameof(CircleBorderThickness), typeof(Thickness), typeof(CircleImage), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CircleImage), new PropertyMetadata(default(CornerRadius)));

        private readonly IFileCacheService _cacheService;
        private readonly string[] _targetHosts = {"pixiv", "pximg"};
        private Border _imageBorder;
        private bool _isInitialized;

        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Brush CircleBorderBrush
        {
            get => (Brush) GetValue(CircleBorderBrushProperty);
            set => SetValue(CircleBorderBrushProperty, value);
        }

        public Thickness CircleBorderThickness
        {
            get => (Thickness) GetValue(CircleBorderThicknessProperty);
            set => SetValue(CircleBorderThicknessProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public CircleImage()
        {
            if (DesignMode.DesignModeEnabled)
                return;
            _cacheService = PrismUnityApplication.Current.Container.Resolve<IFileCacheService>();
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as CircleImage;
            if (e.OldValue == null || e.NewValue == null || !e.OldValue.Equals(e.NewValue))
                control?.SetSource(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            _imageBorder = (Border) GetTemplateChild("ImageBorder");
            _isInitialized = true;
            SetSource(Source);
        }

        private async void SetSource(object source)
        {
            if (!_isInitialized)
                return;
            if (source == null)
                return;

            var uri = source as Uri;
            if (uri == null || IsHttpUri(uri) && !_targetHosts.Any(w => uri.Host.Contains(w)))
                _imageBorder.Background = CreateImageBrush(source.ToString());
            else
                await LoadPixivImageAsync(uri.ToString());
        }

        private async Task LoadPixivImageAsync(string imageUri)
        {
            if (await _cacheService.ExistFileAsync(imageUri))
                _imageBorder.Background = CreateImageBrush(await _cacheService.LoadFileAsync(imageUri));
            else
                _imageBorder.Background = CreateImageBrush(await _cacheService.SaveFileAsync(imageUri));
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }

        private static ImageBrush CreateImageBrush(string str)
        {
            return new ImageBrush {ImageSource = new BitmapImage(new Uri(str))};
        }
    }
}