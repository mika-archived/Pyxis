using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Store;
using Windows.Storage;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class LocalLicenseService : ILicenseService
    {
        private LicenseInformation _licenseInformation;

        public LocalLicenseService()
        {
            _licenseInformation = CurrentAppSimulator.LicenseInformation;
            RunHelper.RunAsync(LocalLicenseServiceCtor);
        }

        #region Implementation of ILicenseService

        public bool IsActivated(string productId)
        {
            return _licenseInformation.ProductLicenses[productId].IsActive;
        }

        #endregion Implementation of ILicenseService

        private async Task LocalLicenseServiceCtor()
        {
            try
            {
                var storeProxy = await ApplicationData.Current.LocalFolder.GetFileAsync("WindowsStoreProxy.xml");
                await CurrentAppSimulator.ReloadSimulatorAsync(storeProxy);
            }
            catch
            {
                // ignored
            }
            _licenseInformation = CurrentAppSimulator.LicenseInformation;
        }
    }
}