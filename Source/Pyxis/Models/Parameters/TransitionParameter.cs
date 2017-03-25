using Newtonsoft.Json;

using Pyxis.Models.Enum;

namespace Pyxis.Models.Parameters
{
    public class TransitionParameter
    {
        public TransitionMode Mode { get; set; }

        public string ToQuery() => JsonConvert.SerializeObject(this);

        public static TransitionParameter FromQuery(string query) => FromQuery<TransitionParameter>(query);

        public static T FromQuery<T>(string query) => JsonConvert.DeserializeObject<T>(query);
    }
}