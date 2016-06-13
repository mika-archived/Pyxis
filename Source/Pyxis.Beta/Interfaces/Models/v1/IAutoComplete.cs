using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     オートコンプリートキーワード
    ///     * API only
    /// </summary>
    public interface IAutoComplete
    {
        IList<string> SearchAutoCompleteKeywords { get; }
    }
}