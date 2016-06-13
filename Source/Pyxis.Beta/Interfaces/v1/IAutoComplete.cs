using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.v1
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