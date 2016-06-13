using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class RankingLabelIllust : IRankingLabelIllust
    {
        #region Implementation of IRankingLabelIllust

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("image_urls")]
        [JsonConverter(typeof(InterfaceToConcreate<ImageUrls>))]
        public IImageUrls ImageUrls { get; set; }

        #endregion
    }
}