using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Illusts : IIllusts
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IIllusts

        [JsonProperty("illusts")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<Illust>>))]
        public IList<IIllust> IllustList { get; set; }

        #endregion
    }
}