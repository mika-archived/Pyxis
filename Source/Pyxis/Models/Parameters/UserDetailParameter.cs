using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class UserDetailParameter : ParameterBase
    {
        [JsonConverter(typeof(InterfaceToConcrete<UserDetail>))]
        public IUserDetail Detail { get; set; }

        public ProfileType ProfileType { get; set; }

        public SearchType SearchType { get; set; }

        public UserDetailParameter Clone()
        {
            return new UserDetailParameter
            {
                Detail = Detail,
                ProfileType = ProfileType,
                SearchType = SearchType
            };
        }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}