using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class SearchResultParameter : ParameterBase
    {
        public string Query { get; set; }

        public SearchSort Sort { get; set; }

        public SearchType SearchType { get; set; }

        public SearchTarget Target { get; set; }

        public SearchDuration Duration { get; set; }

        public string DurationQuery { get; set; }

        public static SearchResultParameter Build(SearchOptionParameter parameter)
        {
            return new SearchResultParameter
            {
                Query = "",
                Sort = parameter.Sort,
                SearchType = parameter.SearchType,
                Target = parameter.Target,
                Duration = parameter.Duration
            };
        }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}