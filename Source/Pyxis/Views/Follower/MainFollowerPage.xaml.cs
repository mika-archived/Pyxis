using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Follower;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Follower
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainFollowerPage : Page
    {
        public MainFollowerPageViewModel ViewModel => DataContext as MainFollowerPageViewModel;

        public MainFollowerPage()
        {
            InitializeComponent();
        }
    }
}