using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Beta.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class UserPreview : IUserPreview
    {
        #region Implementation of IUserPreview

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcrete<User>))]
        public IUser User { get; set; }

        [JsonProperty("illusts")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Illust>>))]
        public IList<IIllust> Illusts { get; set; }

        [JsonProperty("novels")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<Novel>>))]
        public IList<INovel> Novels { get; set; }

        [JsonProperty("is_muted")]
        public bool IsMuted { get; set; }

        #endregion
    }
}