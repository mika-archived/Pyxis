using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;

namespace Pyxis.ViewModels.Favorite
{
    public class IllustMangaFavoritePageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public IllustMangaFavoritePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            RunHelper.RunLater(RedirectToLoginPageWhenNoLogin, TimeSpan.FromMilliseconds(10));
        }

        #endregion

        private void RedirectToLoginPageWhenNoLogin()
        {
            NavigationService.Navigate("Error.LoginRequired", "Favorite.IllustMangaFavorite");
        }
    }
}