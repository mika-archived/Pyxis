using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Search;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Search
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class SearchResultPage : Page
    {
        public SearchResultPageViewModel ViewModel => DataContext as SearchResultPageViewModel;

        public SearchResultPage()
        {
            InitializeComponent();
        }
    }
}