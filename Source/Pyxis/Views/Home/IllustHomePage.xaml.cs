using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Home;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Home
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class IllustHomePage : Page
    {
        public IllustHomePageViewModel ViewModel => DataContext as IllustHomePageViewModel;

        public IllustHomePage()
        {
            InitializeComponent();
            Recommended.Height = ApplicationView.GetForCurrentView().VisibleBounds.Height - 45;
        }
    }
}