using System.Collections.Generic;

namespace Pyxis.Extensions
{
    public static class DictionaryExt
    {
        public static TValue AddAndReturn<TKey, TValue>(this Dictionary<TKey, TValue> obj, TKey key, TValue value)
        {
            obj.Add(key, value);
            return value;
        }
    }
}