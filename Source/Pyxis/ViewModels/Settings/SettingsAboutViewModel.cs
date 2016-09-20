using System;

using Microsoft.Services.Store.Engagement;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Settings
{
    public class SettingsAboutViewModel : ViewModel
    {
        public bool IsSupportFeedback => StoreServicesFeedbackLauncher.IsSupported();

        public async void OnClickedFeedbackLink()
        {
            var launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}