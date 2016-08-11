using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views
{
    public sealed partial class AccountInfoControl : UserControl
    {
        public AccountInfoControlViewModel ViewModel => DataContext as AccountInfoControlViewModel;

        public AccountInfoControl()
        {
            InitializeComponent();
        }
    }
}