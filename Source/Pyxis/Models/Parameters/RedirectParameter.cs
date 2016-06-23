namespace Pyxis.Models.Parameters
{
    internal class RedirectParameter : ParameterBase
    {
        public string RedirectTo { get; set; }

        public ParameterBase Parameter { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;

        #endregion
    }
}