using Windows.UI.Xaml;

namespace Pyxis.Attach
{
    public static class NavigateTo
    {
        public static readonly DependencyProperty PageTokenProperty =
            DependencyProperty.RegisterAttached("PageToken", typeof(string), typeof(NavigateTo),
                                                new PropertyMetadata(string.Empty));

        public static string GetPageToken(DependencyObject obj) => (string) obj.GetValue(PageTokenProperty);

        public static void SetPageToken(DependencyObject obj, string value) => obj.SetValue(PageTokenProperty, value);
    }
}