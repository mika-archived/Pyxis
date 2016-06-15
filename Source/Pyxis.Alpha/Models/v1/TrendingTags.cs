using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class TrendingTags : ITrendingTags
    {
        #region Implementation of ITrendingTags

        [JsonProperty("trend_tags")]
        [JsonConverter(typeof(InterfaceToConcrete<TrendTag>))]
        public IList<ITrendTag> TrendTags { get; set; }

        #endregion
    }
}