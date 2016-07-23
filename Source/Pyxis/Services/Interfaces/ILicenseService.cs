namespace Pyxis.Services.Interfaces
{
    public interface ILicenseService
    {
        bool IsActivated(string productId);
    }
}