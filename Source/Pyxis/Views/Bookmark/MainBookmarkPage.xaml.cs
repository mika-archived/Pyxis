using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Bookmark;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Bookmark
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainBookmarkPage : Page
    {
        public MainBookmarkPageViewModel ViewModel => DataContext as MainBookmarkPageViewModel;

        public MainBookmarkPage()
        {
            InitializeComponent();
        }
    }
}