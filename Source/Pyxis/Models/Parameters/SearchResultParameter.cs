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

        public static SearchResultParameter Build(SearchOptionParameter parameter)
        {
            return new SearchResultParameter
            {
                Query = "",
                Sort = parameter.Sort,
                SearchType = parameter.SearchType,
                Target = parameter.Target,
                Duration = parameter.Duration,
                EitherWord = parameter.EitherWord,
                IgnoreWord = parameter.IgnoreWord,
                BookmarkCount = parameter.BookmarkCount,
                ViewCount = parameter.ViewCount,
                CommentCount = parameter.CommentCount,
                PageCount = parameter.PageCount,
                Height = parameter.Height,
                Width = parameter.Width,
                Tool = parameter.Tool,
                TextLength = parameter.TextLength
            };
        }

        #region AdvancedSearch

        public string EitherWord { get; set; }

        public string IgnoreWord { get; set; }

        public int BookmarkCount { get; set; }

        public int ViewCount { get; set; }

        public int CommentCount { get; set; }

        public int PageCount { get; set; }

        // for Illust/Manga
        public int Height { get; set; }

        public int Width { get; set; }

        public string Tool { get; set; }

        // for Novel
        public int TextLength { get; set; }

        #endregion

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new SearchResultParameter
            {
                Duration = Duration,
                Query = Query,
                SearchType = SearchType,
                Sort = Sort,
                Target = Target,
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