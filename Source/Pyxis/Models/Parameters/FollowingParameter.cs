using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class FollowingParameter : ParameterBase
    {
        public RestrictType Restrict { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new FollowingParameter {Restrict = Restrict};
        }

        #endregion
    }
}