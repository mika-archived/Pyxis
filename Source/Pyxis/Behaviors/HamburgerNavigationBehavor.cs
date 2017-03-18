using Windows.UI.Xaml.Controls;

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    public class HamburgerNavigationBehavor : Behavior<HamburgerMenu>
    {
        protected override void OnAttached()
        {
            AssociatedObject.ItemClick += OnItemClick;
            AssociatedObject.OptionsItemClick += OnItemClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.OptionsItemClick -= OnItemClick;
            AssociatedObject.ItemClick -= OnItemClick;
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var frame = AssociatedObject.Content as Frame;
            if ((e.ClickedItem as HamburgerMenuItem)?.TargetPageType != null)
                frame?.Navigate(((HamburgerMenuItem) e.ClickedItem).TargetPageType);
        }
    }
}