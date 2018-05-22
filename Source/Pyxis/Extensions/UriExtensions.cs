using System;

namespace Pyxis.Extensions
{
    public static class UriExtensions
    {
        public static bool IsHttp(this Uri obj)
        {
            return obj.IsAbsoluteUri && (obj.Scheme == "http" || obj.Scheme == "https");
        }
    }
}