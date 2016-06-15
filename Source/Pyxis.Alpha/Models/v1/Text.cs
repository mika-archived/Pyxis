using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    internal class Text : IText
    {
        #region Implementation of IText

        [JsonProperty("novel_marker")]
        [JsonConverter(typeof(InterfaceToConcreate<NovelMarker>))]
        public INovelMarker NovelMarker { get; set; }

        [JsonProperty("novel_text")]
        public string NovelText { get; set; }

        [JsonProperty("series_prev")]
        public INovel SeriesPrev { get; set; }

        [JsonProperty("series_next")]
        public INovel SeriesNext { get; set; }

        #endregion
    }
}