namespace Pyxis.Services.Interfaces
{
    public interface ICacheService
    {
        void Create(string path);

        void Reference(string path);

        void Update(string path);

        void Delete(string path);

        void Clear();
    }
}