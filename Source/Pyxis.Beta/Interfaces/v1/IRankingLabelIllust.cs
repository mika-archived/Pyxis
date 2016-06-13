namespace Pyxis.Beta.Interfaces.v1
{
    /// <summary>
    ///     ランキングトップページイラスト
    /// </summary>
    public interface IRankingLabelIllust
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     ユーザー名
        /// </summary>
        string UserName { get; }

        /// <summary>
        ///     幅
        /// </summary>
        int Width { get; }

        /// <summary>
        ///     高さ
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     画像 URL
        /// </summary>
        IImageUrls ImageUrls { get; }
    }
}