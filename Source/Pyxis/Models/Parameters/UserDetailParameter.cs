using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Models.Parameters
{
    internal class UserDetailParameter : ParameterBase
    {
        [JsonConverter(typeof(InterfaceToConcrete<User>))]
        public IUser User { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => true;

        #endregion
    }
}