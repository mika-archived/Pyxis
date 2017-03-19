using System;
using System.Collections.Generic;
using System.Windows.Input;

using Windows.ApplicationModel;
using Windows.System;

using Microsoft.Services.Store.Engagement;
using Microsoft.Toolkit.Uwp.Helpers;

using Prism.Commands;

using Pyxis.Models;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class AboutPageViewModel : ViewModel
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public List<Software> Softwares => PyxisConstants.Softwares.Value;

        public AboutPageViewModel()
        {
            Name = SystemInformation.ApplicationName;
            var version = SystemInformation.ApplicationVersion;
            Version = $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        #region OpenFeedbackHubCommand

        private ICommand _openFeedbackHubCommand;
        public ICommand OpenFeedBackHubCommand => _openFeedbackHubCommand ?? (_openFeedbackHubCommand = new DelegateCommand(OpenFeedbackHub));

        private async void OpenFeedbackHub() => await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();

        #endregion

        #region OpenStoreReviewCommand

        private ICommand _openStoreReviewCommand;
        public ICommand OpenStoreReviewCommand => _openStoreReviewCommand ?? (_openStoreReviewCommand = new DelegateCommand(OpenStoreReview));

        private async void OpenStoreReview() =>
            await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Package.Current.Id.FamilyName)));

        #endregion
    }
}