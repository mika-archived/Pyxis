using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Following;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Following
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class PrivateFollowingPage : Page
    {
        public PrivateFollowingPageViewModel ViewModel => DataContext as PrivateFollowingPageViewModel;

        public PrivateFollowingPage()
        {
            InitializeComponent();
        }
    }
}