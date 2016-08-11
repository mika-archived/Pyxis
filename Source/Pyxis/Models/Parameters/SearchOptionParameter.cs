using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class SearchOptionParameter : ParameterBase
    {
        #region Sort

        private SearchSort _sort;

        public SearchSort Sort
        {
            get { return _sort; }
            set { SetProperty(ref _sort, value); }
        }

        #endregion

        #region SearchType

        private SearchType _searchType;

        public SearchType SearchType
        {
            get { return _searchType; }
            set { SetProperty(ref _searchType, value); }
        }

        #endregion

        #region Target

        private SearchTarget _target;

        public SearchTarget Target
        {
            get { return _target; }
            set { SetProperty(ref _target, value); }
        }

        #endregion

        #region Duration

        private SearchDuration _duration;

        public SearchDuration Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        #endregion

        #region Advanced Options

        #region EitherWord

        private string _eitherWord;

        public string EitherWord
        {
            get { return _eitherWord; }
            set { SetProperty(ref _eitherWord, value); }
        }

        #endregion

        #region IgnoreWord

        private string _ignoreWord;

        public string IgnoreWord
        {
            get { return _ignoreWord; }
            set { SetProperty(ref _ignoreWord, value); }
        }

        #endregion

        #region BookmarkCount

        private int _bookmarkCount;

        public int BookmarkCount
        {
            get { return _bookmarkCount; }
            set { SetProperty(ref _bookmarkCount, value); }
        }

        #endregion

        #region ViewCount

        private int _viewCount;

        public int ViewCount
        {
            get { return _viewCount; }
            set { SetProperty(ref _viewCount, value); }
        }

        #endregion

        #region CommentCount

        private int _commentCount;

        public int CommentCount
        {
            get { return _commentCount; }
            set { SetProperty(ref _commentCount, value); }
        }

        #endregion

        #region PageCount

        private int _pageCount;

        public int PageCount
        {
            get { return _pageCount; }
            set { SetProperty(ref _pageCount, value); }
        }

        #endregion

        // for Illust/Manga

        #region Height

        private int _height;

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        #endregion

        #region Width

        private int _width;

        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        #endregion

        #region Tool

        private string _tool;

        public string Tool
        {
            get { return _tool; }
            set { SetProperty(ref _tool, value); }
        }

        #endregion

        // for Novel

        #region TextLength

        private int _textLength;

        public int TextLength
        {
            get { return _textLength; }
            set { SetProperty(ref _textLength, value); }
        }

        #endregion

        #endregion

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new SearchOptionParameter
            {
                Duration = Duration,
                SearchType = SearchType,
                Sort = Sort,
                Target = Target
            };
        }

        #endregion
    }
}