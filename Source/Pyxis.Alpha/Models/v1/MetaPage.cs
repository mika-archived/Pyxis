using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    internal class MetaPage : IMetaPage
    {
        #region Implementation of IMetaPage

        [JsonProperty("image_urls")]
        [JsonConverter(typeof(InterfaceToConcrete<ImageUrls>))]
        public IImageUrls ImageUrls { set; get; }

        #endregion
    }
}