using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class AutoComplete : IAutoComplete
    {
        #region Implementation of IAutoComplete

        [JsonProperty("search_auto_complete_keywords")]
        public IList<string> SearchAutoCompleteKeywords { get; set; }

        #endregion

        #region Implementation of IErrorResponse

        [JsonProperty("error")]
        public dynamic Error { get; set; }

        public bool HasError => Error != null;

        #endregion
    }
}