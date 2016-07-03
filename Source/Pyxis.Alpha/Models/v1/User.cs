using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class User : IUser
    {
        #region Implementation of IUserBase

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account")]
        public string AccountName { get; set; }

        [JsonProperty("profile_image_urls")]
        [JsonConverter(typeof(InterfaceToConcrete<ProfileImageUrls>))]
        public IProfileImageUrls ProfileImageUrls { get; set; }

        #endregion

        #region Implementation of IUser

        [JsonProperty("is_followed")]
        public bool IsFollowed { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        #endregion
    }
}