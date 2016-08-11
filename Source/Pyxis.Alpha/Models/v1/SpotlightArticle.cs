using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class SpotlightArticle : ISpotlightArticle
    {
        #region Implementation of IIdentity

        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region Implementation of ISpotlightArticle

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("article_url")]
        public string ArticleUrl { get; set; }

        [JsonProperty("publish_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime PublishDate { get; set; }

        #endregion
    }
}