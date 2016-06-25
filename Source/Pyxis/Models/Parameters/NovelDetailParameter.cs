using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

using AlphaNovel = Pyxis.Alpha.Models.v1.Novel;

namespace Pyxis.Models.Parameters
{
    internal class NovelDetailParameter : ParameterBase
    {
        [JsonConverter(typeof(InterfaceToConcrete<AlphaNovel>))]
        public INovel Novel { get; set; }

        #region Overrides of ParameterBase

        protected override bool ParseJson => true;
        protected override bool TypeNamingRequired => false;

        #endregion
    }
}