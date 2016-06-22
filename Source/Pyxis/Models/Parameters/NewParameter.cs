using Pyxis.Models.Enums;

namespace Pyxis.Models.Parameters
{
    internal class NewParameter : ParameterBase
    {
        public FollowType FollowType { get; set; }

        public ContentType ContentType { get; set; }
    }
}