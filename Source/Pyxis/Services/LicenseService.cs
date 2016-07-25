using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Store;
using Windows.Storage;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class LicenseService : ILicenseService
    {
#if DEBUG
        private LicenseInformation _licenseInformation;
#else
        private readonly LicenseInformation _licenseInformation;
#endif

        public LicenseService()
        {
#if DEBUG
            RunHelper.RunAsync(Construct);
#else
            _licenseInformation = CurrentApp.LicenseInformation;
#endif
        }

        #region Implementation of ILicenseService

        public bool IsActivated(string productId)
        {
            return _licenseInformation.ProductLicenses[productId].IsActive;
        }

        #endregion

#if DEBUG

        private async Task Construct()
        {
            var storeProxy = await ApplicationData.Current.LocalFolder.GetFileAsync("WindowsStoreProxy.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(storeProxy);
            _licenseInformation = CurrentAppSimulator.LicenseInformation;
        }

#endif
    }
}