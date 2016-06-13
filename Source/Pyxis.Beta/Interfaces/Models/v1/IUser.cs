namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     ユーザー
    /// </summary>
    public interface IUser : IUserBase
    {
        /// <summary>
        ///     フォローしているかどうか
        /// </summary>
        bool IsFollowed { get; }

        /// <summary>
        ///     コメント
        /// </summary>
        string Comment { get; }
    }
}