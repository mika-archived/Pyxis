namespace Pyxis.Gamma
{
    internal static class Endpoints
    {
        private static string BaseUrl => "http://www.pixiv.net";

        private static string SecureBaseUrl => "https://accounts.pixiv.net";

        // private static string Version => "v1";

        // private static string Version => "v2";

        public static string OauthToken => $"{SecureBaseUrl}/login";

        public static string ApplicationInfoIos => ""; // Not supported

        public static string IllustBookmarkUsers => $"{BaseUrl}/member_illust.php";

        public static string IllustBookmarkAdd => $"{BaseUrl}/bookmark_add.php";

        public static string IllustBookmarkDelete => $"";

        public static string IllustComments => $"";

        public static string IllustDetail => $"";

        public static string IllustNew => $"";

        public static string IllustRanking => $"";

        public static string IllustRecommended => $"";

        public static string IllustRecommendedNoLogin => $"";

        public static string IllustRelated => $"";

        public static string MangaRecommended => $"";

        public static string NovelBookmarkAdd => $"";

        public static string NovelBookmarkDelete => $"";

        public static string NovelComments => $"";

        public static string NovelFollow => $"";

        public static string NovelMarkerAdd => $"";

        public static string NovelMarkers => $"";

        public static string NovelMypixiv => $"";

        public static string NovelNew => $"";

        public static string NovelRanking => $"";

        public static string NovelRecommended => $"";

        public static string NovelRecommendedNoLogin => $"";

        public static string NovelSeries => $"";

        public static string NovelText => $"";

        public static string SearchAutoComplete => $"";

        public static string SearchIllust => $"";

        public static string SearchNovel => $"";

        public static string SearchUser => $"";

        public static string SpotlightArticles => $"";

        public static string TrendingTagsIllust => $"";

        public static string TrendingTagsNovel => $"";

        public static string UgoiraMetadata => $"";

        public static string UserBookmarkTagsIllust => $"";

        public static string UserBookmarkTagsNovel => $"";

        public static string UserBookmarksIllust => $"";

        public static string UserBookmarksNovel => $"";

        public static string UserBrowsingHistoryIllusts => $"";

        public static string UserBrowsingHistoryIllustAdd => $"";

        public static string UserBrowsingHistoryNovels => $"";

        public static string UserBrowsingHistoryNovelAdd => $"";

        public static string UserDetail => $"";

        public static string UserFollowAdd => $"";

        public static string UserFollowDelete => $"";

        public static string UserFollower => $"";

        public static string UserFollowing => $"";

        public static string UserIllusts => $"";

        public static string UserList => $"";

        public static string UserMypixiv => $"";

        public static string UserNovels => $"";

        public static string UserRecommended => $"";

        public static string UserRelated => $"";

        // API Version 2

        public static string IllustBookmarkDetail => $"";

        public static string IllustFollow => $"";

        public static string IllustMypixiv => $"";

        public static string NovelBookmarkDetail => $"";
    }
}