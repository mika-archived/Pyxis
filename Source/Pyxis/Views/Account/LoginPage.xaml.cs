using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Account;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Account
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPageViewModel ViewModel => DataContext as LoginPageViewModel;

        public LoginPage()
        {
            InitializeComponent();
        }
    }
}