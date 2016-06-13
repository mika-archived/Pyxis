using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class ProfileImageUrls : IProfileImageUrls
    {
        #region Implementation of IProfileImageUrls

        [JsonProperty("px_16x16")]
        public string Size16 { get; set; }

        [JsonProperty("px_50x50")]
        public string Size50 { get; set; }

        [JsonProperty("px_170x170")]
        public string Size170 { get; set; }

        [JsonProperty("medium")]
        public string Medium
        {
            get { return Size170; }
            set { Size170 = value; }
        }

        #endregion
    }
}