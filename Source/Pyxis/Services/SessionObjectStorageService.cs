using System.Collections.Generic;

using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class SessionObjectStorageService : ISessionObjectStorageService
    {
        private readonly Dictionary<string, object> _storage;

        public SessionObjectStorageService()
        {
            _storage = new Dictionary<string, object>();
        }

        public void AddValue<T>(string identifier, T obj)
        {
            if (_storage.ContainsKey(identifier))
                _storage[identifier] = obj;
            else
                _storage.Add(identifier, obj);
        }

        public T GetValue<T>(string identifier)
        {
            return (T) _storage[identifier];
        }

        public bool ExistValue(string identifier)
        {
            return _storage.ContainsKey(identifier);
        }
    }
}