using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Models.Parameters
{
    internal class IllustDetailParameter : ParameterBase
    {
        public IIllust Illust { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => false;

        #endregion
    }
}