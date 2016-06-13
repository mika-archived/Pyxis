using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Illust : IIllust
    {
        #region Implementation of IIdentity

        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region Implementation of IIllust

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("image_urls")]
        [JsonConverter(typeof(InterfaceToConcreate<ImageUrls>))]
        public IImageUrls ImageUrls { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("restrict")]
        public int Restrict { get; set; }

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcreate<User>))]
        public IUser User { get; set; }

        [JsonProperty("tags")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<Tag>>))]
        public IList<ITag> Tags { get; set; }

        [JsonProperty("tools")]
        public IList<string> Tools { get; set; }

        [JsonProperty("create_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreateDate { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("sanity_level")]
        public int SanityLevel { get; set; }

        [JsonProperty("meta_single_page")]
        [JsonConverter(typeof(InterfaceToConcreate<ImageUrls>))]
        public IImageUrls MetaSinglePage { get; set; }

        [JsonProperty("meta_pages")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<ImageUrls>>))]
        public IList<IImageUrls> MetaPages { get; set; }

        [JsonProperty("total_view")]
        public int TotalView { get; set; }

        [JsonProperty("total_bookmark")]
        public int TotalBookmarks { get; set; }

        [JsonProperty("is_bookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        #endregion
    }
}