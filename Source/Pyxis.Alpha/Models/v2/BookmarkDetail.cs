using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v2;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v2
{
    public class BookmarkDetail : IBookmarkDetail
    {
        #region Implementation of IBookmarkDetail

        [JsonProperty("is_bookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonProperty("tags")]
        [JsonConverter(typeof(InterfaceToConcreate<BookmarkDetailTag>))]
        public IList<IBookmarkDetailTag> Tags { get; set; }

        [JsonProperty("restrict")]
        public string Restrict { get; set; }

        #endregion
    }
}