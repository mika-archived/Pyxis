namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     ユーザー基底インターフェイス
    /// </summary>
    public interface IUserBase
    {
        /// <summary>
        ///     ユーザー数値 ID
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     ユーザー名
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     ユーザーアカウント名
        /// </summary>
        string Account { get; }

        /// <summary>
        ///     プロフィール画像
        /// </summary>
        IProfileImageUrls ProfileImageUrls { get; }
    }
}