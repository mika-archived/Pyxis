using Newtonsoft.Json;

using Pyxis.Beta.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

using AlphaIllust = Pyxis.Alpha.Models.v1.Illust;

namespace Pyxis.Models.Parameters
{
    internal class IllustDetailParameter : ParameterBase
    {
        [JsonConverter(typeof(InterfaceToConcrete<AlphaIllust>))]
        public IIllust Illust { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        public override object Clone()
        {
            return new IllustDetailParameter
            {
                Illust = Illust
            };
        }

        #endregion
    }
}