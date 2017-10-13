using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views
{
    public sealed partial class HamburgerMenuUserControl : UserControl
    {
        public HamburgerMenuUserControlViewModel ViewModel => DataContext as HamburgerMenuUserControlViewModel;

        public HamburgerMenuUserControl()
        {
            InitializeComponent();
        }
    }
}