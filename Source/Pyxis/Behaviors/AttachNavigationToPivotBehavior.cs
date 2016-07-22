using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Xaml.Interactivity;

using Prism.Windows.Navigation;

using Pyxis.Attach;
using Pyxis.Models.Parameters;

namespace Pyxis.Behaviors
{
    internal sealed class AttachNavigationToPivotBehavior : Behavior<Pivot>
    {
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(nameof(NavigationService), typeof(INavigationService),
                                        typeof(AttachNavigationToPivotBehavior),
                                        new PropertyMetadata(null));

        private int _oldSelectedIndex;

        public INavigationService NavigationService
        {
            get { return (INavigationService) GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var pivot = sender as Pivot;
            if (_oldSelectedIndex == pivot?.SelectedIndex)
                return;
            _oldSelectedIndex = AssociatedObject.SelectedIndex;

            var item = pivot?.SelectedItem as PivotItem;
            var pageToken = NavigateTo.GetPageToken(item);
            var param = NavigateTo.GetParameters(item);
            if (param is ParameterBase)
                param = ((ParameterBase) param).ToJson();
            if (pageToken != null)
                NavigationService?.Navigate(pageToken, param);
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            _oldSelectedIndex = AssociatedObject.SelectedIndex;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnDetaching();
        }

        #endregion
    }
}