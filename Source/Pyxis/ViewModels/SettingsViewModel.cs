using System;
using System.Collections.Generic;
using System.Windows.Input;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

using Microsoft.Services.Store.Engagement;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Services;

namespace Pyxis.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        private ICommand _launchFeedbackHubCommand;

        private ICommand _switchThemeCommand;

        private string _versionDescription;

        public Visibility FeedbackLinkVisibility => StoreServicesFeedbackLauncher.IsSupported() ? Visibility.Visible : Visibility.Collapsed;

        public ICommand LaunchFeedbackHubCommand => _launchFeedbackHubCommand ?? (_launchFeedbackHubCommand = new DelegateCommand(LaunchFeedbackHub));

        public ElementTheme ElementTheme
        {
            get => _elementTheme;
            set => SetProperty(ref _elementTheme, value);
        }

        public string VersionDescription
        {
            get => _versionDescription;
            set => SetProperty(ref _versionDescription, value);
        }

        public ICommand SwitchThemeCommand => _switchThemeCommand ?? (_switchThemeCommand = new DelegateCommand<object>(SwitchTheme));

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            VersionDescription = GetVersionDescription();
        }

        private async void SwitchTheme(object param)
        {
            ElementTheme = (ElementTheme) param;
            await ThemeSelectorService.SetThemeAsync((ElementTheme) param);
        }

        private async void LaunchFeedbackHub()
        {
            // This launcher is part of the Store Services SDK https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
            var launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}