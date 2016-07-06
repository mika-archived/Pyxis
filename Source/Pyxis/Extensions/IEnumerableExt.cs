using System;
using System.Collections.Generic;

namespace Pyxis.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExt
    {
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var obj in source)
            {
                action.Invoke(obj);
                yield return obj;
            }
        }
    }
}