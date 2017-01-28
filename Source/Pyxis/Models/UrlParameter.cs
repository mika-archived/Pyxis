using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pyxis.Models
{
    internal static class UrlParameter
    {
        private static readonly Regex ParamRegex = new Regex(@"(.*)\[[0-9]+\]$", RegexOptions.Compiled);

        public static Dictionary<string, string> ParseQuery(string url)
        {
            var dictionary = new Dictionary<string, string>();
            var param = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
            if (param.IndexOf("=", StringComparison.Ordinal) < 0)
                return dictionary;
            while (param != "")
            {
                var kvp = param.IndexOf("&", StringComparison.Ordinal) >= 0
                    ? param.Substring(0, param.IndexOf("&", StringComparison.Ordinal))
                    : param;
                var name = Uri.UnescapeDataString(kvp.Split('=')[0]);
                if (ParamRegex.IsMatch(name))
                    name = $"{ParamRegex.Match(name).Groups[1].Value}[]";
                var value = Uri.UnescapeDataString(kvp.Split('=')[1]);
                if (dictionary.ContainsKey(name))
                {
                    var current = dictionary[name];
                    dictionary[name] = $"{current},{value}"; // Overwrite
                }
                else
                {
                    dictionary[name] = value;
                }
                if (param.IndexOf("&", StringComparison.Ordinal) < 0)
                    break;
                param = param.Substring(param.IndexOf("&", StringComparison.Ordinal) + 1);
            }
            return dictionary;
        }
    }
}