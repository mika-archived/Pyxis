using System;

namespace Pyxis.Models.Enums
{
    internal enum SearchSort
    {
        New,

        // Premium only
        Popular,

        Old
    }

    internal static class SearchSortExt
    {
        public static string ToParamString(this SearchSort sort)
        {
            switch (sort)
            {
                case SearchSort.New:
                    return "date_desc";

                case SearchSort.Popular:
                    return "popular_desc";

                case SearchSort.Old:
                    return "date_asc";

                default:
                    throw new ArgumentOutOfRangeException(nameof(sort), sort, null);
            }
        }
    }
}