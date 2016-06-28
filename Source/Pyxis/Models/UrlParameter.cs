using System;
using System.Collections.Generic;

namespace Pyxis.Models
{
    internal static class UrlParameter
    {
        public static Dictionary<string, string> ParseQuery(string url)
        {
            var dictionary = new Dictionary<string, string>();
            var param = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
            if (param.IndexOf("&", StringComparison.Ordinal) < 0)
                return dictionary;
            while (param != "")
            {
                var kvp = param.IndexOf("&", StringComparison.Ordinal) >= 0
                    ? param.Substring(0, param.IndexOf("&", StringComparison.Ordinal))
                    : param;
                dictionary[kvp.Split('=')[0]] = kvp.Split('=')[1];
                if (param.IndexOf("&", StringComparison.Ordinal) < 0)
                    break;
                param = param.Substring(param.IndexOf("&", StringComparison.Ordinal) + 1);
            }
            return dictionary;
        }
    }
}