namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     自分自身
    /// </summary>
    public interface IAccount : IUserBase
    {
        /// <summary>
        ///     pixiv プレミアム状態
        /// </summary>
        bool IsPremium { get; }

        /// <summary>
        ///     年齢制限
        /// </summary>
        int XRestrict { get; }

        /// <summary>
        ///     メール認証済みか
        /// </summary>
        bool IsMailAuthorized { get; }
    }
}