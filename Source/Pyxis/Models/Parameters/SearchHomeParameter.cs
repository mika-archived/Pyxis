using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class SearchHomeParameter : ParameterBase
    {
        public SearchType SearchType { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}