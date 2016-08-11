using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Tag : ITag
    {
        #region Implementation of ITag

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}