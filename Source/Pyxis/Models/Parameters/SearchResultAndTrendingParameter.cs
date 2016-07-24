using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Alpha.Models.v1;
using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Models.Parameters
{
    internal class SearchResultAndTrendingParameter : SearchResultParameter
    {
        [JsonConverter(typeof(InterfaceToConcrete<Illust>))]
        public IIllust TrendingIllust { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new SearchResultAndTrendingParameter
            {
                Duration = Duration,
                Query = Query,
                SearchType = SearchType,
                Sort = Sort,
                Target = Target,
                TrendingIllust = TrendingIllust,
                EitherWord = EitherWord,
                IgnoreWord = IgnoreWord,
                BookmarkCount = BookmarkCount,
                ViewCount = ViewCount,
                CommentCount = CommentCount,
                PageCount = PageCount,
                Height = Height,
                Width = Width,
                Tool = Tool,
                TextLength = TextLength
            };
        }

        #endregion
    }
}