using System;

namespace Pyxis.Models.Enums
{
    internal enum SearchDuration
    {
        Nothing,

        LastDay,

        LastWeek,

        LastMonth
    }

    internal static class SearchDurationExt
    {
        public static string ToParamString(this SearchDuration duration)
        {
            switch (duration)
            {
                case SearchDuration.Nothing:
                    return "";

                case SearchDuration.LastDay:
                    return "within_last_day";

                case SearchDuration.LastWeek:
                    return "within_last_week";

                case SearchDuration.LastMonth:
                    return "within_last_month";

                default:
                    throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
            }
        }
    }
}