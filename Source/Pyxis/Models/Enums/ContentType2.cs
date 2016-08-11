namespace Pyxis.Models.Enums
{
    internal enum ContentType2
    {
        IllustAndManga,

        Novel
    }

    internal static class ContentType2Ext
    {
        public static ContentType Convert(this ContentType2 contentType)
        {
            if (contentType == ContentType2.IllustAndManga)
                return ContentType.Illust;
            return ContentType.Novel;
        }
    }
}