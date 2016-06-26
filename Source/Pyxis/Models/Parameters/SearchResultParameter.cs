using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class SearchResultParameter : ParameterBase
    {
        public string Query { get; set; }

        public SearchType SearchType { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}