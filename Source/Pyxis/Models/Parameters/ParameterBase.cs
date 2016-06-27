using System;

using Newtonsoft.Json;

using Prism.Mvvm;

namespace Pyxis.Models.Parameters
{
    internal abstract class ParameterBase : BindableBase
    {
        protected abstract bool ParseJson { get; }

        protected abstract bool TypeNamingRequired { get; }

        public object ToJson()
        {
            if (!ParseJson)
                return this;
            if (!TypeNamingRequired)
                return JsonConvert.SerializeObject(this);
            var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            return JsonConvert.SerializeObject(this, jsonSettings);
        }

        public static T ToObject<T>(object json)
        {
            try
            {
                if (json is ParameterBase)
                    return (T) json;
                var jsonString = json.ToString();
                var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
                return JsonConvert.DeserializeObject<T>(jsonString, jsonSettings);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}