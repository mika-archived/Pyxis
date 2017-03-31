using Pyxis.Models.Enum;

using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    internal class IllustParameter : TransitionParameter
    {
        public Illust Illust { get; set; }

        public int Id { get; set; }

        public bool HasObject => Illust != null;

        public IllustParameter()
        {
            Mode = TransitionMode.Forward;
        }
    }
}