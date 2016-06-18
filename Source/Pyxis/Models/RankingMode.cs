using System;

namespace Pyxis.Models
{
    public enum RankingMode
    {
        Daily,

        DailyMale,

        DailyFemale,

        WeeklyOriginal,

        WeeklyRookie,

        Weekly,

        Monthly
    }

    public static class RankingModeExt
    {
        public static RankingMode FromString(string str)
        {
            switch (str)
            {
                case "day":
                case "daily":
                    return RankingMode.Daily;

                case "day_male":
                case "daily_male":
                    return RankingMode.DailyMale;

                case "day_female":
                case "daily_female":
                    return RankingMode.DailyFemale;

                case "week_original":
                case "weekly_original":
                    return RankingMode.WeeklyOriginal;

                case "week_rookie":
                case "weekly_rookie":
                    return RankingMode.WeeklyRookie;

                case "week":
                case "weekly":
                    return RankingMode.Weekly;

                case "month":
                case "monthly":
                    return RankingMode.Monthly;

                default:
                    throw new ArgumentOutOfRangeException(nameof(str), str, null);
            }
        }

        public static string ToDisplayString(this RankingMode mode)
        {
            switch (mode)
            {
                case RankingMode.Daily:
                    return "デイリーランキング";

                case RankingMode.DailyMale:
                    return "男子に人気";

                case RankingMode.DailyFemale:
                    return "女子に人気";

                case RankingMode.WeeklyOriginal:
                    return "オリジナル";

                case RankingMode.WeeklyRookie:
                    return "ルーキー";

                case RankingMode.Weekly:
                    return "ウィークリー";

                case RankingMode.Monthly:
                    return "マンスリー";

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}