using Windows.UI.Xaml;

namespace Pyxis.Attach
{
    public static class NavigateTo
    {
        #region Parameters

        public static readonly DependencyProperty ParametersProperty =
            DependencyProperty.RegisterAttached("Parameters", typeof(object), typeof(NavigateTo),
                                                new PropertyMetadata(null));

        public static object GetParameters(DependencyObject obj) => obj.GetValue(ParametersProperty);

        public static void SetParameters(DependencyObject obj, object value) => obj.SetValue(ParametersProperty, value);

        #endregion

        #region PageToken

        public static readonly DependencyProperty PageTokenProperty =
            DependencyProperty.RegisterAttached("PageToken", typeof(string), typeof(NavigateTo),
                                                new PropertyMetadata(string.Empty));

        public static string GetPageToken(DependencyObject obj) => (string) obj.GetValue(PageTokenProperty);

        public static void SetPageToken(DependencyObject obj, string value) => obj.SetValue(PageTokenProperty, value);

        #endregion
    }
}