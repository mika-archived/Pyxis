using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Pyxis.Alpha.Converters
{
    public class InterfaceToConcrete<T> : JsonConverter where T : class
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var value = serializer.Deserialize<T>(reader);
            if (typeof(T).Name == "IList`1") // Collection
            {
                var type = objectType.GenericTypeArguments.First();
                var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
                foreach (var v in (IList) value)
                    list.Add(v);
                return list;
            }
            return value;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}