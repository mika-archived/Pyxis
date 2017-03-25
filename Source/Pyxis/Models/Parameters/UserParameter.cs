using Pyxis.Models.Enum;

namespace Pyxis.Models.Parameters
{
    internal class UserParameter : TransitionParameter
    {
        public int UserId { get; set; }

        public UserParameter()
        {
            Mode = TransitionMode.Redirect;
        }
    }
}