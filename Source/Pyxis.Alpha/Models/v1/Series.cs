using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class Series : ISeries
    {
        #region Implementation of IIdentity

        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion

        #region Implementation of ISeries

        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion
    }
}