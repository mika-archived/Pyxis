using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class FavoriteOptionParameter : ParameterBase
    {
        public string UserId { get; set; } = "";

        public SearchType Type { get; set; }

        public RestrictType Restrict { get; set; }

        public string Tag { get; set; } = "";

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new FavoriteOptionParameter
            {
                UserId = UserId,
                Type = Type,
                Restrict = Restrict,
                Tag = Tag
            };
        }

        #endregion
    }
}