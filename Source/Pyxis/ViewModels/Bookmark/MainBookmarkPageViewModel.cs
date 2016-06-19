using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Helpers;

namespace Pyxis.ViewModels.Bookmark
{
    public class MainBookmarkPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public MainBookmarkPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
            _navigationService.Navigate("Error.LoginRequired", "Bookmark.MainBookmark");
        }
    }
}