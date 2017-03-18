using Windows.UI.Xaml;

using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Pyxis.Controls
{
    internal class HamburgerMenuImageItem : HamburgerMenuItem
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(string), typeof(HamburgerMenuImageItem), new PropertyMetadata(null));

        public string Image
        {
            get { return (string) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}