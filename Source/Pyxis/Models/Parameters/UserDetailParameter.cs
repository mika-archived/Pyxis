using Pyxis.Models.Enums;

using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    internal class UserDetailParameter : ParameterBase
    {
        public UserDetail Detail { get; set; }

        public ProfileType ProfileType { get; set; }

        public ContentType ContentType { get; set; }

        public override object Clone()
        {
            return new UserDetailParameter
            {
                Detail = Detail,
                ProfileType = ProfileType,
                ContentType = ContentType
            };
        }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}