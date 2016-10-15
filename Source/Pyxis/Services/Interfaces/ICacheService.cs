using System.Threading.Tasks;

using Pyxis.Models.Cache;

namespace Pyxis.Services.Interfaces
{
    public interface ICacheService
    {
        Task CreateAsync(string path, long size);

        Task<CacheFile> ReferenceAsync(string path);

        Task UpdateAsync(CacheFile cache);

        Task DeleteAsync(CacheFile cache);

        Task ClearAsync();
    }
}