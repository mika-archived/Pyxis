using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Bookmark : IBookmark
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IBookmark

        [JsonProperty("users")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<User>>))]
        public IList<IUser> Users { get; set; }

        #endregion
    }
}