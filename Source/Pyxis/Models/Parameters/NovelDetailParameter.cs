using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    internal class NovelDetailParameter : ParameterBase
    {
        public Novel Novel { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new NovelDetailParameter {Novel = Novel};
        }

        #endregion
    }
}