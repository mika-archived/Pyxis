using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Work;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Work
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class NovelWorkPage : Page
    {
        public NovelWorkPageViewModel ViewModel => DataContext as NovelWorkPageViewModel;

        public NovelWorkPage()
        {
            InitializeComponent();
        }
    }
}