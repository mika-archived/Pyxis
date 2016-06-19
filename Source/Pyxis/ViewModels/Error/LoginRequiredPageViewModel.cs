﻿using System.Collections.Generic;

using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Error
{
    public class LoginRequiredPageViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;

        public LoginRequiredPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            ClearNavigationHistory();
        }

        #endregion

        private void ClearNavigationHistory()
        {
            _navigationService.RemoveLastPage();
        }
    }
}