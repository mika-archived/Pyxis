using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

using Microsoft.Toolkit.Uwp.UI.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Pyxis.Controls
{
    /// <summary>
    ///     Pixiv の画像に
    /// </summary>
    [TemplatePart(Name = "ImageControl", Type = typeof(ImageEx))]
    public sealed class PixivImageEx : PixivControl<ImageEx>
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

        public override object Source
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
        }

        public event ImageExOpenedEventHandler ImageExOpened;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ImageControl.ImageExOpened += (sender, e) => ImageExOpened?.Invoke(sender, e);
        }

        protected override void AssignToImageControl(string image)
        {
            ImageControl.Source = image;
        }
    }
}