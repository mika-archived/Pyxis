using Newtonsoft.Json;

namespace Pyxis.Models.Parameters
{
    internal abstract class ParameterBase
    {
        protected abstract bool ParseJson { get; }

        public object ToJson(bool typeNaming = false)
        {
            if (!ParseJson)
                return this;
            if (!typeNaming)
                return JsonConvert.SerializeObject(this);
            var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            return JsonConvert.SerializeObject(this, jsonSettings);
        }

        public static T ToObject<T>(object json, bool typeNaming = false)
        {
            try
            {
                if (json is ParameterBase)
                    return (T) json;
                var jsonString = json.ToString();
                if (!typeNaming)
                    return JsonConvert.DeserializeObject<T>(jsonString);
                var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
                return JsonConvert.DeserializeObject<T>(jsonString, jsonSettings);
            }
            catch
            {
                return default(T);
            }
        }
    }
}