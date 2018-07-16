using System;

using Pyxis.Enums;

using Sagitta.Models;

namespace Pyxis.Models.Parameters
{
    public class DetailsParameter : TransitionParameter
    {
        public long Id { get; set; }

        public Work Work { get; set; }

        public Type Type { get; set; }

        public DetailsParameter()
        {
            Mode = TransitionMode.Forward;
        }

        protected override bool Validate()
        {
            return Id != 0 || Work != null;
        }
    }
}