using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Novel : INovel
    {
        #region Implementation of IIdentity

        [JsonProperty("Id")]
        public int Id { get; set; }

        #endregion

        #region Implementation of INovel

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("restrict")]
        public int Restrict { get; set; }

        [JsonProperty("image_urls")]
        [JsonConverter(typeof(InterfaceToConcrete<ImageUrls>))]
        public IImageUrls ImageUrls { get; set; }

        [JsonProperty("create_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreateDate { get; set; }

        [JsonProperty("tags")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Tag>>))]
        public IList<ITag> Tags { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("text_length")]
        public int TextLength { get; set; }

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcrete<User>))]
        public IUser User { get; set; }

        [JsonProperty("series")]
        [JsonConverter(typeof(InterfaceToConcrete<Series>))]
        public ISeries Series { get; set; }

        [JsonProperty("is_bookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonProperty("total_bookmarks")]
        public int TotalBookmarks { get; set; }

        [JsonProperty("total_view")]
        public int TotalView { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("total_comments")]
        public int TotalComments { get; set; }

        #endregion
    }
}