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
using Pyxis.Services.Interfaces;
using Pyxis.Views;

namespace Pyxis.ViewModels.Base
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        protected IAccountService AccountService { get; }

        protected INavigationService NavigationService { get; }

        protected ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
            AccountService = PrismUnityApplication.Current.Container.Resolve<IAccountService>();
            NavigationService = PrismUnityApplication.Current.Container.Resolve<INavigationService>();
        }

        #region Implementation of IDisposable

        public void Dispose() => CompositeDisposable.Dispose();

        #endregion

        protected void RedirectTo(string pageToken, TransitionParameter parameter)
        {
            RunHelper.RunLaterUI(() => NavigationService.Navigate(pageToken, parameter.ToQuery()), TimeSpan.FromMilliseconds(1));
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
                                              bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (suspending)
                CompositeDisposable.Dispose();
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
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
                    NavigationService.RemoveLastPage();
                    NavigationService.RemoveLastPage();
                }
            }

            // Back Button
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack()
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        #endregion
    }
}