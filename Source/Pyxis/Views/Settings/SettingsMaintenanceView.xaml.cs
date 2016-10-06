using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Settings;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views.Settings
{
    public sealed partial class SettingsMaintenanceView : UserControl
    {
        public SettingsMaintenanceViewModel ViewModel => DataContext as SettingsMaintenanceViewModel;

        public SettingsMaintenanceView()
        {
            InitializeComponent();
        }
    }
}