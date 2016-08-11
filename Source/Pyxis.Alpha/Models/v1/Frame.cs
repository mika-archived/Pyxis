using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Frame : IFrame
    {
        #region Implementation of IFrame

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }

        #endregion
    }
}