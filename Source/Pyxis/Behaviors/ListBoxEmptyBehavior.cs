using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    internal class ListBoxEmptyBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(FrameworkElement), typeof(ListBoxEmptyBehavior),
                                        new PropertyMetadata(null));

        private int _count;
        private bool _isAttached;

        public FrameworkElement Target
        {
            get { return (FrameworkElement) GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        private void AssociatedObjectOnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (AssociatedObject.Items == null)
                return;
            if (!_isAttached)
            {
                _isAttached = true;
                AssociatedObject.Items.VectorChanged += ItemsOnVectorChanged;
            }
        }

        private void ItemsOnVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
        {
            switch (e.CollectionChange)
            {
                case CollectionChange.ItemInserted:
                    _count++;
                    break;

                case CollectionChange.ItemRemoved:
                    _count--;
                    break;

                case CollectionChange.ItemChanged:
                    break;

                case CollectionChange.Reset:
                    _count = 0;
                    break;
            }
            if (_count > 0)
            {
                AssociatedObject.Visibility = Visibility.Visible;
                if (Target != null)
                    Target.Visibility = Visibility.Collapsed;
            }
            else
            {
                // AssociatedObject.Visibility = Visibility.Collapsed;
                if (Target != null)
                    Target.Visibility = Visibility.Visible;
            }
        }

        #region Overrides of Behavior<ItemsControl>

        protected override void OnAttached()
        {
            base.OnAttached();
            _count = 0;
            AssociatedObject.DataContextChanged += AssociatedObjectOnDataContextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DataContextChanged -= AssociatedObjectOnDataContextChanged;
            if (AssociatedObject.Items != null && _isAttached)
                AssociatedObject.Items.VectorChanged -= ItemsOnVectorChanged;
        }

        #endregion
    }
}