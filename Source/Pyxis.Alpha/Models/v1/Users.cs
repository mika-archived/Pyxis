using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Beta.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Users : IUsers
    {
        #region Implementation of IIndex

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }

        #endregion

        #region Implementation of IBookmark

        [JsonProperty("users")]
        [JsonConverter(typeof(InterfaceToConcrete<IList<User>>))]
        public IList<IUser> UserList { get; set; }

        #endregion

        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}