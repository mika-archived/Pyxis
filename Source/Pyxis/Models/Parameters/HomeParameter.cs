using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    /// <summary>
    ///     Home/* に渡す Parameter
    /// </summary>
    internal class HomeParameter : ParameterBase
    {
        public ContentType ContentType { get; set; }
    }
}