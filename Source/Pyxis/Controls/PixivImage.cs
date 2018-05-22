using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Pyxis.Controls
{
    [TemplatePart(Name = "ImageControl", Type = typeof(Image))]
    public sealed class PixivImage : PixivControl<Image>
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(PixivImage), new PropertyMetadata(default(object), OnSourceChanged));

        public static readonly DependencyProperty StretchProperty
            = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PixivImage), new PropertyMetadata(Stretch.Fill));

        public Stretch Stretch
        {
            get => (Stretch) GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public override object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public PixivImage()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            DefaultStyleKey = typeof(PixivImageEx);
        }

        protected override void AssignToImageControl(string image)
        {
            ImageControl.Source = CreateImageSource(image);
        }
    }
}