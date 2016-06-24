using System;

using Newtonsoft.Json;

namespace Pyxis.Models.Parameters
{
    internal abstract class ParameterBase
    {
        protected abstract bool ParseJson { get; }

        public object ToJson()
        {
            if (!ParseJson)
                return this;
            return JsonConvert.SerializeObject(this);
        }

        public static T ToObject<T>(object json)
        {
            try
            {
                if (json is ParameterBase)
                    return (T) json;
                var jsonString = json.ToString();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}