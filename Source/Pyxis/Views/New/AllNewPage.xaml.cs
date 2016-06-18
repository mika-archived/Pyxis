using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.New;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.New
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class AllNewPage : Page
    {
        public AllNewPageViewModel ViewModel => DataContext as AllNewPageViewModel;

        public AllNewPage()
        {
            InitializeComponent();
        }
    }
}