using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class SearchOptionParameter : ParameterBase
    {
        public SearchSort Sort { get; set; }

        public SearchType SearchType { get; set; }

        public SearchTarget Target { get; set; }

        public SearchDuration Duration { get; set; }

        public string DurationQuery { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}