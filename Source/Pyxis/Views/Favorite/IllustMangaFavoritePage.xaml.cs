using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Favorite;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views.Favorite
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class IllustMangaFavoritePage : Page
    {
        public IllustMangaFavoritePageViewModel ViewModel => DataContext as IllustMangaFavoritePageViewModel;

        public IllustMangaFavoritePage()
        {
            InitializeComponent();
        }
    }
}