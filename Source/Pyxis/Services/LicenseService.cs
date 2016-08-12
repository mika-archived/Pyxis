using Windows.ApplicationModel.Store;

using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class LicenseService : ILicenseService
    {
        private readonly LicenseInformation _licenseInformation;

        public LicenseService()
        {
            _licenseInformation = CurrentApp.LicenseInformation;
        }

        #region Implementation of ILicenseService

        public bool IsActivated(string productId)
        {
            return _licenseInformation.ProductLicenses[productId].IsActive;
        }

        #endregion
    }
}