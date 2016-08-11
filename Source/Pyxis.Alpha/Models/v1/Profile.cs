using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Profile : IProfile
    {
        #region Implementation of IProfile

        [JsonProperty("webpage")]
        public string Webpage { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("birth")]
        public string Birth { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("total_follow_users")]
        public int TotalFollowUsers { get; set; }

        [JsonProperty("total_follower")]
        public int TotalFollower { get; set; }

        [JsonProperty("total_mypixiv_users")]
        public int TotalMypixivUsers { get; set; }

        [JsonProperty("total_illusts")]
        public int TotalIllusts { get; set; }

        [JsonProperty("total_manga")]
        public int TotalManga { get; set; }

        [JsonProperty("total_novels")]
        public int TotalNovels { get; set; }

        [JsonProperty("background_image_url")]
        public string BackgroundImageUrl { get; set; }

        [JsonProperty("twitter_account")]
        public string TwitterAccount { get; set; }

        [JsonProperty("twitter_account_url")]
        public string TwitterAccountUrl { get; set; }

        #endregion
    }
}