using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    internal class IllustDetailParameter : ParameterBase
    {
        public Illust Illust { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new IllustDetailParameter
            {
                Illust = Illust
            };
        }

        #endregion
    }
}