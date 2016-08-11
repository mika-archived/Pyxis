using Windows.UI.Xaml;

namespace Pyxis.Attach
{
    public static class AssumSize
    {
        public static readonly DependencyProperty PageTokenProperty =
            DependencyProperty.RegisterAttached("AssumSize", typeof(string), typeof(AssumSize),
                                                new PropertyMetadata(""));

        public static string GetAssumSize(DependencyObject obj) => (string) obj.GetValue(PageTokenProperty);

        public static void SetAssumSize(DependencyObject obj, string value) => obj.SetValue(PageTokenProperty, value);
    }
}