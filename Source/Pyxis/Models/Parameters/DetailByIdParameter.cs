namespace Pyxis.Models.Parameters
{
    internal class DetailByIdParameter : ParameterBase
    {
        public string Id { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}