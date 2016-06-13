using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Novels : INovels
    {
        #region Implementation of IIndex

        [JsonProperty("next_ur;")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of INovels

        [JsonProperty("novels")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<Novel>>))]
        public IList<INovel> NovelList { get; set; }

        #endregion
    }
}