using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class BookmarkTags : IBookmarkTags
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IBookmarkTags

        [JsonProperty("bookmark_tags")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<BookmarkTag>>))]
        public IList<IBookmarkTag> BookmarkTagList { get; set; }

        #endregion

        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}