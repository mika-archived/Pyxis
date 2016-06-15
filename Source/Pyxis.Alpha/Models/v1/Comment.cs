using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Comment : IComment
    {
        #region Implementation of IIdentity

        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region Implementation of IComment

        [JsonProperty("comment")]
        public string CommentBody { get; set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcrete<UserBase>))]
        public IUserBase User { get; set; }

        #endregion
    }
}