using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Ranking;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Ranking
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MangaRankingPage : Page
    {
        public MangaRankingPageViewModel ViewModel => DataContext as MangaRankingPageViewModel;

        public MangaRankingPage()
        {
            InitializeComponent();
        }
    }
}