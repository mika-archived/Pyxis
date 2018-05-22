using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IFileCacheStorage
    {
        Task<string> SaveFileAsync(string url);

        Task<bool> ExistFileAsync(string url);

        Task<string> LoadFileAsync(string url);

        Task ClearAsync();

        Task SaveFileToLocalAsync(string url, string local);
    }
}