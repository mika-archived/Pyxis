using Newtonsoft.Json;

namespace Pyxis.Models.Parameters
{
    internal class ParameterBase
    {
        public string ToJson() => JsonConvert.SerializeObject(this);

        public static T ToObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
    }
}