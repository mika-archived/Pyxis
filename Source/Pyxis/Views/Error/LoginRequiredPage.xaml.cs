using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Error;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Error
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class LoginRequiredPage : Page
    {
        public LoginRequiredPageViewModel ViewModel => DataContext as LoginRequiredPageViewModel;

        public LoginRequiredPage()
        {
            InitializeComponent();
        }
    }
}