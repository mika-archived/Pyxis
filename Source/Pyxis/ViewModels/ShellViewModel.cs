using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Helpers;
using Pyxis.Services;
using Pyxis.Services.Interfaces;
using Pyxis.Views;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private NavigationView _navigationView;
        private NavigationViewItem _selected;

        public ICommand ItemInvokedCommand { get; }

        public ReactiveProperty<string> AppTitle { get; }
        public ReactiveProperty<string> ViewTitle { get; }

        public NavigationViewItem Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ShellViewModel(INavigationService navigationServiceInstance, ITitleService titleService)
        {
            _navigationService = navigationServiceInstance;
            ItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked);

            AppTitle = (titleService as TitleService).ObserveProperty(w => w.AppTitle).Where(w => !string.IsNullOrWhiteSpace(w)).ToReactiveProperty().AddTo(this);
            ViewTitle = (titleService as TitleService).ObserveProperty(w => w.ViewTitle).Where(w => !string.IsNullOrWhiteSpace(w)).ToReactiveProperty().AddTo(this);
            ViewTitle.Subscribe(w => { Debug.WriteLine(w); });

            titleService.AppTitle = "Home";
            titleService.ViewTitle = "Home";
        }

        public void Initialize(Frame frame, NavigationView navigationView)
        {
            _navigationView = navigationView;
            frame.Navigated += Frame_Navigated;
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate("Settings", null);
                return;
            }

            var item = _navigationView.MenuItems
                                      .OfType<NavigationViewItem>()
                                      .First(menuItem => (string) menuItem.Content == (string) args.InvokedItem);
            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
            _navigationService.Navigate(pageKey, null);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                                      .OfType<NavigationViewItem>()
                                      .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourcePageKey = sourcePageType.ToString().Split('.').Last().Replace("Page", string.Empty);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == sourcePageKey;
        }
    }
}