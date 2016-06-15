using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    internal class ZipUrls : IZipUrls
    {
        #region Implementation of IZipUrls

        [JsonProperty("medium")]
        public string Medium { get; set; }

        #endregion
    }
}