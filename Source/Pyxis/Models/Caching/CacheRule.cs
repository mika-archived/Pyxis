using Newtonsoft.Json;

namespace Pyxis.Models.Caching
{
    public class CacheRule
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public T GetTypedValue<T>() => JsonConvert.DeserializeObject<T>(Value);

        public void SetTypedValue<T>(T value) => Value = JsonConvert.SerializeObject(value);
    }
}