namespace Pyxis.Models.Database
{
    public enum CacheType
    {
        /// <summary>
        ///     ユーザープロフィール画像
        /// </summary>
        ProfileImage = 0,

        /// <summary>
        ///     投稿作品画像
        /// </summary>
        MasterImage = 1,

        /// <summary>
        ///     小説カバーイラスト
        /// </summary>
        NovelCover = 2,

        /// <summary>
        ///     ワークスペース画像
        /// </summary>
        Workspace = 3,

        /// <summary>
        ///     ユーザープロフィール背景画像
        /// </summary>
        ProfileBackground = 4,

        /// <summary>
        ///     うごイラ ZIP ファイル
        /// </summary>
        UgoiraZip = 5,

        /// <summary>
        ///     デフォルト画像
        /// </summary>
        DefaultImages = 6,

        /// <summary>
        ///     その他
        /// </summary>
        Others = 10
    }
}