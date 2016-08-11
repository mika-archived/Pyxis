using System;

namespace Pyxis.Models.Enums
{
    public enum ContentType
    {
        Illust,

        Manga,

        Novel,

        User
    }

    public static class ContentTypeExt
    {
        public static SearchType ToSearchType(this ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Illust:
                case ContentType.Manga:
                    return SearchType.IllustsAndManga;

                case ContentType.Novel:
                    return SearchType.Novels;

                case ContentType.User:
                    return SearchType.Users;

                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
            }
        }
    }
}