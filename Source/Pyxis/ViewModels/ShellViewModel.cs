﻿using System;
using System.Linq;
using System.Windows.Input;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Pyxis.Helpers;
using Pyxis.Views;

namespace Pyxis.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private NavigationView _navigationView;
        private NavigationViewItem _selected;

        public ICommand ItemInvokedCommand { get; }

        public NavigationViewItem Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ShellViewModel(INavigationService navigationServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            ItemInvokedCommand = new DelegateCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked);
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