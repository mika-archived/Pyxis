using System;

using Newtonsoft.Json;

namespace Pyxis.Alpha.Converters
{
    internal class InterfaceToConcreate<T> : JsonConverter where T : class
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}