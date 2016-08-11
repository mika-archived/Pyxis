using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v2;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v2
{
    public class BookmarkDetailTag : IBookmarkDetailTag
    {
        #region Implementation of ITag

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Implementation of IBookmarkDetailTag

        [JsonProperty("is_registered")]
        public bool IsRegistered { get; set; }

        #endregion
    }
}