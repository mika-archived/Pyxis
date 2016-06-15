using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    internal class NovelMarker : INovelMarker
    {
        #region Implementation of INovelMarker

        [JsonProperty("page")]
        public int Page { get; set; }

        #endregion
    }
}