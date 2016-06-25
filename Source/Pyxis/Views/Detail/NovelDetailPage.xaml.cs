using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Detail;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Detail
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class NovelDetailPage : Page
    {
        public NovelDetailPageViewModel ViewModel => DataContext as NovelDetailPageViewModel;

        public NovelDetailPage()
        {
            InitializeComponent();
        }
    }
}