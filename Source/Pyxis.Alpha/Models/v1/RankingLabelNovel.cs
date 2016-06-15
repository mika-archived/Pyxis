using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class RankingLabelNovel : IRankingLabelNovel
    {
        #region Implementation of IRankingLabelNovel

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image_urls")]
        [JsonConverter(typeof(InterfaceToConcrete<ImageUrls>))]
        public IImageUrls ImageUrls { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        #endregion
    }
}