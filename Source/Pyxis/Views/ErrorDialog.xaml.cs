using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views
{
    public sealed partial class ErrorDialog : ContentDialog
    {
        public ErrorDialogViewModel ViewModel => DataContext as ErrorDialogViewModel;

        public ErrorDialog()
        {
            InitializeComponent();
        }
    }
}