namespace Pyxis.Alpha
{
    internal static class Endpoints
    {
        private static string BaseUrl => "https://app-api.pixiv.net";

        private static string BaseAuthUrl => "https://oauth.secure.pixiv.net";

        private static string Version1 => "v1";

        private static string Version2 => "v2";

        internal static string OauthToken => $"{BaseAuthUrl}/auth/token";

        internal static string ApplicationInfoIos => $"{BaseUrl}/{Version1}/application-info/ios";

        internal static string IllustBookmarkUsers => $"{BaseUrl}/{Version1}/illust/bookmark/users";

        internal static string IllustBookmarkAdd => $"{BaseUrl}/{Version1}/illust/bookmark/add";

        internal static string IllustComments => $"{BaseUrl}/{Version1}/illust/comments";

        internal static string IllustNew => $"{BaseUrl}/{Version1}/illust/new";

        internal static string IllustRanking => $"{BaseUrl}/{Version1}/illust/ranking";

        internal static string IllustRecommended => $"{BaseUrl}/{Version1}/illust/recommended";

        internal static string IllustRecommendedNoLogin => $"{BaseUrl}/{Version1}/illust/recommended-nologin";

        internal static string IllustRelated => $"{BaseUrl}/{Version1}/illust/related";

        internal static string MangaRecommended => $"{BaseUrl}/{Version1}/manga/recommended";

        internal static string NovelBookmarkAdd => $"{BaseUrl}/{Version1}/novel/bookmark/add";

        internal static string NovelComments => $"{BaseUrl}/{Version1}/novel/comments";

        internal static string NovelFollow => $"{BaseUrl}/{Version1}/novel/follow";

        internal static string NovelMarkerAdd => $"{BaseUrl}/{Version1}/novel/marker/add";

        internal static string NovelMarkers => $"{BaseUrl}/{Version1}/novel/markers";

        internal static string NovelMypixiv => $"{BaseUrl}/{Version1}/novel/mypixiv";

        internal static string NovelNew => $"{BaseUrl}/{Version1}/novel/new";

        internal static string NovelRanking => $"{BaseUrl}/{Version1}/novel/ranking";

        internal static string NovelRecommended => $"{BaseUrl}/{Version1}/novel/recommended";

        internal static string NovelRecommendedNoLogin => $"{BaseUrl}/{Version1}/novel/recommended-nologin";

        internal static string NovelSeries => $"{BaseUrl}/{Version1}/novel/series";

        internal static string NovelText => $"{BaseUrl}/{Version1}/novel/text";

        internal static string SearchAutocomplete => $"{BaseUrl}/{Version1}/search/autocomplete";

        internal static string SearchIllust => $"{BaseUrl}/{Version1}/search/illust";

        internal static string SearchNovel => $"{BaseUrl}/{Version1}/search/novel";

        internal static string SearchUser => $"{BaseUrl}/{Version1}/search/user";

        internal static string SpotlightArticles => $"{BaseUrl}/{Version1}/spotlight/articles";

        internal static string TrendingTagsIllust => $"{BaseUrl}/{Version1}/trending-tags/illust";

        internal static string TrendingTagsNovel => $"{BaseUrl}/{Version1}/trending-tags/novel";

        internal static string UgoiraMetadata => $"{BaseUrl}/{Version1}/ugoira/metadata";

        internal static string UserBookmarkTagsIllust => $"{BaseUrl}/{Version1}/user/bookmark-tags/illust";

        internal static string UserBookmarksIllust => $"{BaseUrl}/{Version1}/user/bookmarks/illust";

        internal static string UserBookmarksNovel => $"{BaseUrl}/{Version1}/user/bookmarks/novel";

        internal static string UserBrowsingHistoryIllustAdd => $"{BaseUrl}/{Version1}/user/browsing-history/illust/add";

        internal static string UserBrowsingHistoryNovelAdd => $"{BaseUrl}/{Version1}/user/browsin-history/novel/add";

        internal static string UserDetail => $"{BaseUrl}/{Version1}/user/detail";

        internal static string UserFollowAdd => $"{BaseUrl}/{Version1}/user/follow/add";

        internal static string UserFollower => $"{BaseUrl}/{Version1}/user/follower";

        internal static string UserFollowing => $"{BaseUrl}/{Version1}/user/following";

        internal static string UserIllusts => $"{BaseUrl}/{Version1}/user/illusts";

        internal static string UserMypixiv => $"{BaseUrl}/{Version1}/user/mypixiv";

        internal static string UserNovels => $"{BaseUrl}/{Version1}/user/novels";

        internal static string UserRecommended => $"{BaseUrl}/{Version1}/user/recommended";

        internal static string UserRelated => $"{BaseUrl}/{Version1}/user/related";

        // API Version 2

        internal static string IllustBookmarkDetail => $"{BaseUrl}/{Version2}/illust/bookmark/detail";

        internal static string IllustFollow => $"{BaseUrl}/{Version2}/illust/follow";

        internal static string IllustMypixiv => $"{BaseUrl}/{Version2}/illust/mypixiv";

        internal static string NovelBookmarkDetail => $"{BaseUrl}/{Version2}/novel/bookmark/detail";
    }
}