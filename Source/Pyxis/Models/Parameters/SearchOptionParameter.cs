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

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}