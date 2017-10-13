using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using Windows.UI.Core;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Models.Enum;
using Pyxis.Models.Parameters;
using Pyxis.Navigation;
using Pyxis.Services.Interfaces;
using Pyxis.Views;

namespace Pyxis.ViewModels.Base
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        private readonly INavigationService _navigationService;
        public CompositeDisposable CompositeDisposable { get; }

        protected IAccountService AccountService { get; }

        protected ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
            AccountService = PrismUnityApplication.Current.Container.Resolve<IAccountService>();
            _navigationService = PrismUnityApplication.Current.Container.Resolve<INavigationService>();
        }

        #region Implementation of IDisposable

        public void Dispose() => CompositeDisposable.Dispose();

        #endregion

        protected void RedirectTo(string pageToken, TransitionParameter parameter)
        {
            RunHelper.RunLaterUI(() => _navigationService.Navigate(pageToken, parameter.ToQuery()), TimeSpan.FromMilliseconds(1));
        }

        protected void NavigateTo(string pageToken, TransitionParameter parameter) => _navigationService.Navigate(pageToken, parameter.ToQuery());

        public virtual void OnNavigatingFrom(PyxisNavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending) {}

        public virtual void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState) {}

        #region Overrides of ViewModelBase

        public sealed override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
                                                     bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            OnNavigatingFrom(new PyxisNavigatingFromEventArgs(e), viewModelState, suspending);
            CompositeDisposable.Dispose();
        }

        public sealed override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            // Redirect to
            if (AccountService.Account == null && e.SourcePageType != typeof(LoginPage))
            {
                RedirectTo("Login", new TransitionParameter {Mode = TransitionMode.Redirect});
                return;
            }

            // When natigated to this by redirect, remove backstack history.
            if (e.Parameter != null)
            {
                var parameter = TransitionParameter.FromQuery(e.Parameter.ToString());
                if (parameter.Mode == TransitionMode.Redirect)
                {
                    // 1: This page, 2: Last (redirect source) page.
                    _navigationService.RemoveLastPage();
                    _navigationService.RemoveLastPage();
                }
            }

            // Back Button
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _navigationService.CanGoBack()
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;

            OnNavigatedTo(new PyxisNavigatedToEventArgs(e), viewModelState);
        }

        #endregion
    }
}