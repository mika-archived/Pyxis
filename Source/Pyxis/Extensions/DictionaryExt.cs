using System.Collections.Generic;

namespace Pyxis.Extensions
{
    internal static class DictionaryExt
    {
// ReSharper disable  InconsistentNaming
        /// <summary>
        ///     Dictionary&lt;K, V&gt; にキー key が含まれる場合はその値を、
        ///     含まれていない場合は default(V) を返します。
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static V TryGet<K, V>(this Dictionary<K, V> obj, K key) => obj.ContainsKey(key) ? obj[key] : default(V);
    }
}