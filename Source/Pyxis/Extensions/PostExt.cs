using Sagitta.Models;

namespace Pyxis.Extensions
{
    public static class PostExt
    {
        public static string GetIdentifier(this Post obj)
        {
            return obj is Illust ? "Illust" : "Novel";
        }
    }
}