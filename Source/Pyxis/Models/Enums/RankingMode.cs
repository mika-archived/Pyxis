using System;

using Windows.ApplicationModel.Resources;

namespace Pyxis.Models.Enums
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
        private static readonly ResourceLoader Resources = ResourceLoader.GetForCurrentView();

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
                    return Resources.GetString("Daily/Text");

                case RankingMode.DailyMale:
                    return Resources.GetString("PopularMale/Text");

                case RankingMode.DailyFemale:
                    return Resources.GetString("PopularFemale/Text");

                case RankingMode.WeeklyOriginal:
                    return Resources.GetString("Original/Text");

                case RankingMode.WeeklyRookie:
                    return Resources.GetString("Rookie/Text");

                case RankingMode.Weekly:
                    return Resources.GetString("Weekly/Text");

                case RankingMode.Monthly:
                    return Resources.GetString("Monthly/Text");

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ToParamString(this RankingMode mode, ContentType type = ContentType.Illust)
        {
            switch (mode)
            {
                case RankingMode.Daily:
                    if ((type == ContentType.Illust) || (type == ContentType.Novel))
                        return "day";
                    return "day_manga";

                case RankingMode.DailyMale:
                    if (type == ContentType.Manga)
                        throw new NotSupportedException();
                    return "day_male";

                case RankingMode.DailyFemale:
                    if (type == ContentType.Manga)
                        throw new NotSupportedException();
                    return "day_female";

                case RankingMode.WeeklyOriginal:
                    if (type != ContentType.Illust)
                        throw new NotSupportedException();
                    return "week_original";

                case RankingMode.WeeklyRookie:
                    if ((type == ContentType.Illust) || (type == ContentType.Novel))
                        return "week_rookie";
                    return "week_rookie_manga";

                case RankingMode.Weekly:
                    if ((type == ContentType.Illust) || (type == ContentType.Novel))
                        return "week";
                    return "week_manga";

                case RankingMode.Monthly:
                    if (type == ContentType.Novel)
                        throw new NotSupportedException();
                    if (type == ContentType.Illust)
                        return "month";
                    return "month_manga";

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static int ToParamIndex(this RankingMode mode, ContentType type = ContentType.Illust)
        {
            if (type == ContentType.User)
                throw new NotSupportedException();

            switch (mode)
            {
                case RankingMode.Daily:
                    return 0;

                case RankingMode.DailyMale:
                    if (type == ContentType.Manga)
                        throw new NotSupportedException();
                    return 1;

                case RankingMode.DailyFemale:
                    if (type == ContentType.Manga)
                        throw new NotSupportedException();
                    return 2;

                case RankingMode.WeeklyOriginal:
                    if (type == ContentType.Illust)
                        return 3;
                    throw new NotSupportedException();

                case RankingMode.WeeklyRookie:
                    if (type == ContentType.Manga)
                        return 1;
                    if (type == ContentType.Novel)
                        return 3;
                    return 4;

                case RankingMode.Weekly:
                    if (type == ContentType.Manga)
                        return 2;
                    if (type == ContentType.Novel)
                        return 4;
                    return 5;

                case RankingMode.Monthly:
                    if (type == ContentType.Manga)
                        return 3;
                    if (type == ContentType.Novel)
                        throw new NotSupportedException();
                    return 6;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}