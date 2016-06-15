using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class RecommendedIllusts : IRecommendedIllusts
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IRecommendedIllusts

        [JsonProperty("illusts")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Illust>>))]
        public IList<IIllust> Illusts { get; set; }

        [JsonProperty("home_ranking_illusts")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Illust>>))]
        public IList<IIllust> HomeRankingIllusts { get; set; }

        [JsonProperty("ranking_label_illust")]
        [JsonConverter(typeof(InterfaceToConcrete<RankingLabelIllust>))]
        public IRankingLabelIllust RankingLabelIllust { get; set; }

        #endregion
    }
}