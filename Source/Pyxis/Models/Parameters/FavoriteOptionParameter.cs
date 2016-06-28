using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class FavoriteOptionParameter : ParameterBase
    {
        public SearchType Type { get; set; }

        public RestrictType Restrict { get; set; }

        public string Tag { get; set; }

        public FavoriteOptionParameter Clone()
        {
            return new FavoriteOptionParameter
            {
                Type = Type,
                Restrict = Restrict,
                Tag = Tag
            };
        }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}