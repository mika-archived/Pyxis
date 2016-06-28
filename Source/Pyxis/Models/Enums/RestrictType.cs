using System;

namespace Pyxis.Models.Enums
{
    internal enum RestrictType
    {
        All,

        Public,

        Private
    }

    public static class RestrictTypeExt
    {
        internal static string ToParamString(this RestrictType restrict)
        {
            switch (restrict)
            {
                case RestrictType.All:
                    return "all";

                case RestrictType.Public:
                    return "public";

                case RestrictType.Private:
                    return "private";

                default:
                    throw new ArgumentOutOfRangeException(nameof(restrict), restrict, null);
            }
        }
    }
}