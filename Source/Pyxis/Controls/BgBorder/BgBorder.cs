using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    public sealed class BgBorder : ContentControl
    {
        public static readonly DependencyProperty BorderBackgroundProperty =
            DependencyProperty.Register(nameof(BorderBackground), typeof(Brush), typeof(BgBorder), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(BgBorder), new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty BorderOpacityProperty =
            DependencyProperty.Register(nameof(BorderOpacity), typeof(double), typeof(BgBorder), new PropertyMetadata(default(double)));

        public Brush BorderBackground
        {
            get { return (Brush) GetValue(BorderBackgroundProperty); }
            set { SetValue(BorderBackgroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public double BorderOpacity
        {
            get { return (double) GetValue(BorderOpacityProperty); }
            set { SetValue(BorderOpacityProperty, value); }
        }

        public BgBorder()
        {
            DefaultStyleKey = typeof(BgBorder);
        }
    }
}