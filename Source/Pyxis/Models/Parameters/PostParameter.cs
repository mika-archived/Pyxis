using Pyxis.Models.Enum;

using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    internal class PostParameter<T> : TransitionParameter where T : Post
    {
        public T Post { get; set; }

        public int Id { get; set; }

        public bool HasObject => Post != null;

        public PostParameter()
        {
            Mode = TransitionMode.Forward;
        }
    }
}