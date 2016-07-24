using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class BrowsingHistoryParameter : ParameterBase
    {
        public ContentType2 ContentType { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new BrowsingHistoryParameter {ContentType = ContentType};
        }

        #endregion
    }
}