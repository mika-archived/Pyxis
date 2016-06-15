using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Metadata : IMetadata
    {
        #region Implementation of IMetadata

        [JsonProperty("zip_urls")]
        [JsonConverter(typeof(InterfaceToConcrete<ZipUrls>))]
        public IZipUrls ZipUrls { get; set; }

        [JsonProperty("frames")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Frame>>))]
        public IList<IFrame> Frames { get; set; }

        #endregion
    }
}