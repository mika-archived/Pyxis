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
        public string Name { get; }
        public string Version { get; }
        public List<Software> Softwares => PyxisConstants.Softwares.Value;

        public AboutPageViewModel()
        {
            Name = SystemInformation.ApplicationName;
            var version = SystemInformation.ApplicationVersion;
            Version = $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            if (!string.IsNullOrWhiteSpace(PyxisConstants.Branch))
                Version += $" - {PyxisConstants.Branch}";
        }

        #region OpenFeedbackHubCommand

        private ICommand _openFeedbackHubCommand;
        public ICommand OpenFeedBackHubCommand => _openFeedbackHubCommand ?? (_openFeedbackHubCommand = new DelegateCommand(OpenFeedbackHub));

        private async void OpenFeedbackHub()
        {
            await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
        }

        #endregion

        #region OpenStoreReviewCommand

        private ICommand _openStoreReviewCommand;
        public ICommand OpenStoreReviewCommand => _openStoreReviewCommand ?? (_openStoreReviewCommand = new DelegateCommand(OpenStoreReview));

        private async void OpenStoreReview()
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:REVIEW?PFN={Package.Current.Id.FamilyName}"));
        }

        #endregion
    }
}