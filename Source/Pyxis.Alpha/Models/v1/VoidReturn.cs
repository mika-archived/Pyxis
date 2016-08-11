using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    internal class VoidReturn : IVoidReturn
    {
        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}