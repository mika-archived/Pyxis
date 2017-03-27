using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IFileCacheService
    {
        Task<string> SaveFileAsync(string url);

        Task<bool> ExistFileAsync(string url);

        Task<string> LoadFileAsync(string url);

        Task CleanAsync();

        Task SaveFileToLocalAsync(string url, string dist);
    }
}