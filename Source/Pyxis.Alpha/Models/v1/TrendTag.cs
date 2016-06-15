using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class TrendTag : ITrendTag
    {
        #region Implementation of ITrendTag

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("illust")]
        [JsonConverter(typeof(InterfaceToConcrete<Illust>))]
        public IIllust Illust { get; set; }

        #endregion
    }
}