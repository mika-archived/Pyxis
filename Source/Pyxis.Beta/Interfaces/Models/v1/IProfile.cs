namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     ユーザープロファイル
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        ///     HP アドレス
        /// </summary>
        string Webpage { get; }

        /// <summary>
        ///     性別
        /// </summary>
        string Gender { get; }

        /// <summary>
        ///     誕生日
        /// </summary>
        string Birth { get; }

        /// <summary>
        ///     住所
        /// </summary>
        string Region { get; }

        /// <summary>
        ///     職業
        /// </summary>
        string Job { get; }

        /// <summary>
        ///     フォローユーザー数
        /// </summary>
        int TotalFollowUsers { get; }

        /// <summary>
        ///     フォロワー数
        /// </summary>
        int TotalFollower { get; }

        /// <summary>
        ///     マイピク数
        /// </summary>
        int TotalMypixivUsers { get; }

        /// <summary>
        ///     イラスト数
        /// </summary>
        int TotalIllusts { get; }

        /// <summary>
        ///     漫画数
        /// </summary>
        int TotalManga { get; }

        /// <summary>
        ///     小説数
        /// </summary>
        int TotalNovels { get; }

        /// <summary>
        ///     背景画像 URL
        /// </summary>
        string BackgroundImageUrl { get; }

        /// <summary>
        ///     Twitter アカウント
        /// </summary>
        string TwitterAccount { get; }

        /// <summary>
        ///     Twitter アカウント URL
        /// </summary>
        string TwitterAccountUrl { get; }
    }
}