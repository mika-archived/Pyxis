using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    public sealed class AdaptiveGridViewEx : ContentControl
    {
        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.Register(nameof(DesiredWidth), typeof(double), typeof(AdaptiveGridViewEx), new PropertyMetadata(200));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(AdaptiveGridViewEx), new PropertyMetadata(200));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(UIElement), typeof(AdaptiveGridViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty FixedHeaderProperty =
            DependencyProperty.Register(nameof(FixedHeader), typeof(UIElement), typeof(AdaptiveGridViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(AdaptiveGridViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(AdaptiveGridViewEx), new PropertyMetadata(null));

        public double DesiredWidth
        {
            get { return (double) GetValue(DesiredWidthProperty); }
            set { SetValue(DesiredWidthProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double) GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public UIElement Header
        {
            get { return (UIElement) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public UIElement FixedHeader
        {
            get { return (UIElement) GetValue(FixedHeaderProperty); }
            set { SetValue(FixedHeaderProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public AdaptiveGridViewEx()
        {
            DefaultStyleKey = typeof(AdaptiveGridViewEx);
        }
    }
}