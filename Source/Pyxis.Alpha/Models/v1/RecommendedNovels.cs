using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class RecommendedNovels : IRecommendedNovels
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IRecommendedNovels

        [JsonProperty("novels")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Novel>>))]
        public IList<INovel> Novels { get; set; }

        [JsonProperty("home_ranking_novels")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Novel>>))]
        public IList<INovel> HomeRankingNovels { get; set; }

        [JsonProperty("ranking_label_novel")]
        [JsonConverter(typeof(InterfaceToConcrete<RankingLabelNovel>))]
        public IRankingLabelNovel RankingLabelNovel { get; set; }

        #endregion

        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}