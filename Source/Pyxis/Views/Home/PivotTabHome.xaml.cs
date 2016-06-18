using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Prism.Windows.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views.Home
{
    public sealed partial class PivotTabHome : UserControl
    {
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(PivotTabHome),
                                        new PropertyMetadata(0, PropertyChangedCallback));

        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(nameof(NavigationService), typeof(INavigationService), typeof(PivotTabHome),
                                        new PropertyMetadata(null));

        private bool _isHandling;

        public int SelectedIndex
        {
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public INavigationService NavigationService
        {
            get { return (INavigationService) GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

        public PivotTabHome()
        {
            InitializeComponent();
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var obj = sender as PivotTabHome;
            if (obj != null)
            {
                obj._isHandling = true;
                obj.SelectedIndex = (int) e.NewValue;
                obj.Pivot.SelectedIndex = (int) e.NewValue;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            if (pivot?.SelectedIndex == SelectedIndex || _isHandling)
            {
                _isHandling = false;
                return;
            }
            var item = pivot?.SelectedItem as PivotItem;
            if (!string.IsNullOrWhiteSpace((string) item?.Tag))
                NavigationService?.Navigate((string) item.Tag, null);
        }
    }
}