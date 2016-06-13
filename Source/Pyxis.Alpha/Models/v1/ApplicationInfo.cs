using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class ApplicationInfo : IApplicationInfo
    {
        #region Implementation of IApplicationInfo

        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; }

        [JsonProperty("update_required")]
        public bool UpdateRequired { get; set; }

        [JsonProperty("update_available")]
        public bool UpdateAvailable { get; set; }

        [JsonProperty("update_message")]
        public string UpdateMessage { get; set; }

        [JsonProperty("notice_exists")]
        public bool NoticeExists { get; set; }

        [JsonProperty("store_url")]
        public string StoreUrl { get; set; }

        [JsonProperty("notice_id")]
        public string NoticeId { get; set; }

        [JsonProperty("notice_important")]
        public bool NoticeImportant { get; set; }

        [JsonProperty("notice_message")]
        public string NoticeMessage { get; set; }

        #endregion
    }
}