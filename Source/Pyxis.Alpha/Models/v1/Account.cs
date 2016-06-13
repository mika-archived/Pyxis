using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Account : IAccount
    {
        #region Implementation of IUserBase

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account")]
        public string AccountName { get; set; }

        [JsonProperty("profile_image_urls")]
        [JsonConverter(typeof(InterfaceToConcreate<ProfileImageUrls>))]
        public IProfileImageUrls ProfileImageUrls { get; set; }

        #endregion

        #region Implementation of IAccount

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        [JsonProperty("x_restrict")]
        public int XRestrict { get; set; }

        [JsonProperty("is_mail_authorized")]
        public bool IsMailAuthorized { get; set; }

        #endregion
    }
}