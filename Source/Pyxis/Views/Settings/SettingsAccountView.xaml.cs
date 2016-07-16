using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Settings;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Settings
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class SettingsAccountView : Page
    {
        public SettingsAccountViewModel ViewModel => DataContext as SettingsAccountViewModel;

        public SettingsAccountView()
        {
            InitializeComponent();
        }
    }
}