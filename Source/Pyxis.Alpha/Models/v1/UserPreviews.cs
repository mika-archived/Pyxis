using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class UserPreviews : IUserPreviews
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IUserPreviews

        [JsonProperty("user_previews")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<UserPreview>>))]
        public IList<IUserPreview> UserPreviewList { get; set; }

        #endregion
    }
}