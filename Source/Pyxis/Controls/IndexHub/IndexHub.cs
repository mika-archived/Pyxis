using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    public sealed class IndexHub : Control
    {
        public static readonly DependencyProperty HubHeaderTextProperty =
            DependencyProperty.Register(nameof(HubHeaderText), typeof(string), typeof(IndexHub), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(IndexHub), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(IndexHub), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HubContentTemplateProperty =
            DependencyProperty.Register(nameof(HubContentTemplate), typeof(DataTemplate), typeof(IndexHub), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(IndexHub), new PropertyMetadata(default(DataTemplate)));

        public string HubHeaderText
        {
            get { return (string) GetValue(HubHeaderTextProperty); }
            set { SetValue(HubHeaderTextProperty, value); }
        }

        public string HeaderText
        {
            get { return (string) GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate HubContentTemplate
        {
            get { return (DataTemplate) GetValue(HubContentTemplateProperty); }
            set { SetValue(HubContentTemplateProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public IndexHub()
        {
            DefaultStyleKey = typeof(IndexHub);
        }
    }
}