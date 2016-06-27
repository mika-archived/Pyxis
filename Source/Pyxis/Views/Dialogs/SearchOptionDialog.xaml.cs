using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Dialogs;

//コンテンツ ダイアログ項目テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください。

namespace Pyxis.Views.Dialogs
{
    public sealed partial class SearchOptionDialog : ContentDialog
    {
        public SearchOptionDialogViewModel ViewModel => DataContext as SearchOptionDialogViewModel;

        public SearchOptionDialog()
        {
            InitializeComponent();
        }
    }
}