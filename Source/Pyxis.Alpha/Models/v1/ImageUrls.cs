using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class ImageUrls : IImageUrls
    {
        #region Implementation of IImageUrls

        [JsonProperty("square_medium")]
        public string SquareMedium { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }

        #region Original

        [JsonProperty("original")]
        public string Original { get; set; }

        [JsonProperty("original_image_url")]
        public string OriginalImageUrl
        {
            get { return Original; }
            set { Original = value; }
        }

        #endregion

        #endregion
    }
}