using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Workspace : IWorkspace
    {
        #region Implementation of IWorkspace

        [JsonProperty("pc")]
        public string Pc { get; set; }

        [JsonProperty("monitor")]
        public string Monitor { get; set; }

        [JsonProperty("tool")]
        public string Tool { get; set; }

        [JsonProperty("scanner")]
        public string Scanner { get; set; }

        [JsonProperty("tablet")]
        public string Tablet { get; set; }

        [JsonProperty("mouse")]
        public string Mouse { get; set; }

        [JsonProperty("printer")]
        public string Printer { get; set; }

        [JsonProperty("desktop")]
        public string Desktop { get; set; }

        [JsonProperty("music")]
        public string Music { get; set; }

        [JsonProperty("desk")]
        public string Desk { get; set; }

        [JsonProperty("chair")]
        public string Chair { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("workspace_image_url")]
        public string WorkspaceImageUrl { get; set; }

        #endregion
    }
}