using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views.Contents
{
    public sealed partial class TagsArea : UserControl
    {
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(nameof(Tags), typeof(object), typeof(TagsArea),
                                        new PropertyMetadata(default(object)));

        public object Tags
        {
            get { return GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        public TagsArea()
        {
            InitializeComponent();
        }
    }
}