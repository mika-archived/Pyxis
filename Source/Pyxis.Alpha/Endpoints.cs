namespace Pyxis.Alpha
{
    public static class Endpoints
    {
        private static string BaseUrl => "https://app-api.pixiv.net";

        private static string BaseAuthUrl => "https://oauth.secure.pixiv.net";

        private static string Version1 => "v1";

        private static string Version2 => "v2";

        public static string OauthToken => $"{BaseAuthUrl}/auth/token";

        public static string ApplicationInfoIos => $"{BaseUrl}/{Version1}/application-info/ios";

        public static string IllustBookmarkUsers => $"{BaseUrl}/{Version1}/illust/bookmark/users";

        public static string IllustBookmarkAdd => $"{BaseUrl}/{Version1}/illust/bookmark/add";

        public static string IllustComments => $"{BaseUrl}/{Version1}/illust/comments";

        public static string IllustNew => $"{BaseUrl}/{Version1}/illust/new";

        public static string IllustRanking => $"{BaseUrl}/{Version1}/illust/ranking";

        public static string IllustRecommended => $"{BaseUrl}/{Version1}/illust/recommended";

        public static string IllustRecommendedNoLogin => $"{BaseUrl}/{Version1}/illust/recommended-nologin";

        public static string IllustRelated => $"{BaseUrl}/{Version1}/illust/related";

        public static string MangaRecommended => $"{BaseUrl}/{Version1}/manga/recommended";

        public static string NovelBookmarkAdd => $"{BaseUrl}/{Version1}/novel/bookmark/add";

        public static string NovelComments => $"{BaseUrl}/{Version1}/novel/comments";

        public static string NovelFollow => $"{BaseUrl}/{Version1}/novel/follow";

        public static string NovelMarkerAdd => $"{BaseUrl}/{Version1}/novel/marker/add";

        public static string NovelMarkers => $"{BaseUrl}/{Version1}/novel/markers";

        public static string NovelMypixiv => $"{BaseUrl}/{Version1}/novel/mypixiv";

        public static string NovelNew => $"{BaseUrl}/{Version1}/novel/new";

        public static string NovelRanking => $"{BaseUrl}/{Version1}/novel/ranking";

        public static string NovelRecommended => $"{BaseUrl}/{Version1}/novel/recommended";

        public static string NovelRecommendedNoLogin => $"{BaseUrl}/{Version1}/novel/recommended-nologin";

        public static string NovelSeries => $"{BaseUrl}/{Version1}/novel/series";

        public static string NovelText => $"{BaseUrl}/{Version1}/novel/text";

        public static string SearchAutocomplete => $"{BaseUrl}/{Version1}/search/autocomplete";

        public static string SearchIllust => $"{BaseUrl}/{Version1}/search/illust";

        public static string SearchNovel => $"{BaseUrl}/{Version1}/search/novel";

        public static string SearchUser => $"{BaseUrl}/{Version1}/search/user";

        public static string SpotlightArticles => $"{BaseUrl}/{Version1}/spotlight/articles";

        public static string TrendingTagsIllust => $"{BaseUrl}/{Version1}/trending-tags/illust";

        public static string TrendingTagsNovel => $"{BaseUrl}/{Version1}/trending-tags/novel";

        public static string UgoiraMetadata => $"{BaseUrl}/{Version1}/ugoira/metadata";

        public static string UserBookmarkTagsIllust => $"{BaseUrl}/{Version1}/user/bookmark-tags/illust";

        public static string UserBookmarksIllust => $"{BaseUrl}/{Version1}/user/bookmarks/illust";

        public static string UserBookmarksNovel => $"{BaseUrl}/{Version1}/user/bookmarks/novel";

        public static string UserBrowsingHistoryIllustAdd => $"{BaseUrl}/{Version1}/user/browsing-history/illust/add";

        public static string UserBrowsingHistoryNovelAdd => $"{BaseUrl}/{Version1}/user/browsin-history/novel/add";

        public static string UserDetail => $"{BaseUrl}/{Version1}/user/detail";

        public static string UserFollowAdd => $"{BaseUrl}/{Version1}/user/follow/add";

        public static string UserFollower => $"{BaseUrl}/{Version1}/user/follower";

        public static string UserFollowing => $"{BaseUrl}/{Version1}/user/following";

        public static string UserIllusts => $"{BaseUrl}/{Version1}/user/illusts";

        public static string UserMypixiv => $"{BaseUrl}/{Version1}/user/mypixiv";

        public static string UserNovels => $"{BaseUrl}/{Version1}/user/novels";

        public static string UserRecommended => $"{BaseUrl}/{Version1}/user/recommended";

        public static string UserRelated => $"{BaseUrl}/{Version1}/user/related";

        // API Version 2

        public static string IllustBookmarkDetail => $"{BaseUrl}/{Version2}/illust/bookmark/detail";

        public static string IllustFollow => $"{BaseUrl}/{Version2}/illust/follow";

        public static string IllustMypixiv => $"{BaseUrl}/{Version2}/illust/mypixiv";

        public static string NovelBookmarkDetail => $"{BaseUrl}/{Version2}/novel/bookmark/detail";
    }
}