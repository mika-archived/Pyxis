using Newtonsoft.Json;

using Pyxis.Models.Enum;

namespace Pyxis.Models
{
    public class TransitionParameter
    {
        public TransitionMode Mode { get; set; }

        public string ToQuery() => JsonConvert.SerializeObject(this);

        public static TransitionParameter FromQuery(string query) => JsonConvert.DeserializeObject<TransitionParameter>(query);
    }
}