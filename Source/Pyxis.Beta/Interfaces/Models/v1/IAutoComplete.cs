using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     オートコンプリートキーワード
    ///     * API only
    /// </summary>
    public interface IAutoComplete : IErrorResponse
    {
        IList<string> SearchAutoCompleteKeywords { get; }
    }
}