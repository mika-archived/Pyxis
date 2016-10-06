using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IImageStoreService
    {
        Task<string> SaveImageAsync(string url);

        Task<string> LoadImageAsync(string url);

        Task<bool> ExistImageAsync(string url);

        Task ClearImagesAsync();

        Task SaveToLocalFolderAsync(string url);
    }
}