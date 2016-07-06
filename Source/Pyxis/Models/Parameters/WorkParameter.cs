using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class WorkParameter : ParameterBase
    {
        public ContentType ContentType { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new WorkParameter {ContentType = ContentType};
        }

        #endregion
    }
}