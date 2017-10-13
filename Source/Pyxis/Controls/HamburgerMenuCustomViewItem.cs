using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Pyxis.Controls
{
    [ContentProperty(Name = "CustomView")]
    public class HamburgerMenuCustomViewItem : HamburgerMenuItem
    {
        public UIElement CustomView { get; set; }
    }
}