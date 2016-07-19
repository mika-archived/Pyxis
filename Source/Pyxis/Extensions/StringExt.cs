using System;
using System.Linq;

namespace Pyxis.Extensions
{
    public static class StringExt
    {
        public static string FirstCharToUpper(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException(nameof(str));
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}