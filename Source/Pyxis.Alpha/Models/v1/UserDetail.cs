using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class UserDetail : IUserDetail
    {
        #region Implementation of IUserDetail

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcrete<User>))]
        public IUser User { get; set; }

        [JsonProperty("profile")]
        [JsonConverter(typeof(InterfaceToConcrete<Profile>))]
        public IProfile Profile { get; set; }

        [JsonProperty("workspace")]
        [JsonConverter(typeof(InterfaceToConcrete<Workspace>))]
        public IWorkspace Workspace { get; set; }

        #endregion
    }
}