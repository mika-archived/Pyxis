namespace Pyxis.Services.Interfaces
{
    public interface ISessionObjectStorageService
    {
        void AddValue<T>(string identifier, T obj);

        T GetValue<T>(string identifier);

        bool ExistValue(string identifier);
    }
}