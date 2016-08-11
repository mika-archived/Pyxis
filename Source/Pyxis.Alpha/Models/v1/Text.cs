using Newtonsoft.Json;

using Pyxis.Beta.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Text : IText
    {
        #region Implementation of IText

        [JsonProperty("novel_marker")]
        [JsonConverter(typeof(InterfaceToConcrete<NovelMarker>))]
        public INovelMarker NovelMarker { get; set; }

        [JsonProperty("novel_text")]
        public string NovelText { get; set; }

        [JsonProperty("series_prev")]
        [JsonConverter(typeof(InterfaceToConcrete<Novel>))]
        public INovel SeriesPrev { get; set; }

        [JsonProperty("series_next")]
        [JsonConverter(typeof(InterfaceToConcrete<Novel>))]
        public INovel SeriesNext { get; set; }

        #endregion

        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}