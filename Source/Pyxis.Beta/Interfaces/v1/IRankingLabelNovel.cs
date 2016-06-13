namespace Pyxis.Beta.Interfaces.v1
{
    public interface IRankingLabelNovel
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     画像 URL
        /// </summary>
        IImageUrls ImageUrls { get; }

        /// <summary>
        ///     ユーザー名
        /// </summary>
        string UserName { get; }
    }
}