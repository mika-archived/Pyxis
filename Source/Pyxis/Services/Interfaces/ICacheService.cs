using Pyxis.Models.Cache;

namespace Pyxis.Services.Interfaces
{
    public interface ICacheService
    {
        void Create(string path, long size);

        CacheFile Reference(string path);

        void Update(string path, long size);

        void Delete(string path);

        void Clear();
    }
}