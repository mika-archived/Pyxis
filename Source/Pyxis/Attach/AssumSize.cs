using Windows.UI.Xaml;

namespace Pyxis.Attach
{
    public static class AssumSize
    {
        public static readonly DependencyProperty PageTokenProperty =
            DependencyProperty.RegisterAttached("AssumSize", typeof(int), typeof(AssumSize),
                                                new PropertyMetadata(-1));

        public static int GetAssumSize(DependencyObject obj) => (int) obj.GetValue(PageTokenProperty);

        public static void SetAssumSize(DependencyObject obj, int value) => obj.SetValue(PageTokenProperty, value);
    }
}