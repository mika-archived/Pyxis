using Windows.ApplicationModel.Store;

using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class LicenseService : ILicenseService
    {
        private readonly LicenseInformation _licenseInformation;

        public LicenseService()
        {
#if DEBUG
            _licenseInformation = CurrentAppSimulator.LicenseInformation;
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
    }
}