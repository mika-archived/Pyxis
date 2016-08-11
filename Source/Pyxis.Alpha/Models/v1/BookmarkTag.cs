using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class BookmarkTag : IBookmarkTag
    {
        #region Implementation of ITag

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Implementation of IBookmarkTag

        [JsonProperty("count")]
        public int Count { get; set; }

        #endregion
    }
}