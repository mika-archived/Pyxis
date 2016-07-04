namespace Pyxis.Models.Parameters
{
    internal class UserDetailParameter : ParameterBase
    {
        public string Id { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}