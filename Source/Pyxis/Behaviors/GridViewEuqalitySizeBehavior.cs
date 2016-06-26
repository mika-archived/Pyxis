using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Xaml.Interactivity;

using Pyxis.Attach;

namespace Pyxis.Behaviors
{
    internal class GridViewEuqalitySizeBehavior : Behavior<ItemsWrapGrid>
    {
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;
            var assumSize = AssumSize.GetAssumSize(AssociatedObject);

            var maxColumn = Math.Floor(size.Width / assumSize);
            var adjustedSize = assumSize + size.Width % assumSize / maxColumn;

            AssociatedObject.ItemHeight = adjustedSize;
            AssociatedObject.ItemWidth = adjustedSize;
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
            base.OnDetaching();
        }

        #endregion
    }
}