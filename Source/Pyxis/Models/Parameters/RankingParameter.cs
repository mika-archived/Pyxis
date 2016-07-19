using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class RankingParameter : ParameterBase
    {
        public ContentType RankingType { get; set; }
        public RankingMode RankingMode { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new RankingParameter
            {
                RankingType = RankingType,
                RankingMode = RankingMode
            };
        }

        #endregion
    }
}