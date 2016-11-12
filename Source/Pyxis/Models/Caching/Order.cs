namespace Pyxis.Models.Caching
{
    internal enum Order
    {
        /// <summary>
        ///     作成日 (昇順)
        /// </summary>
        CreatedAtAsc,

        /// <summary>
        ///     作成日 (降順)
        /// </summary>
        CreatedAtDesc,

        /// <summary>
        ///     更新日 (昇順)
        /// </summary>
        UpdatedAtAsc,

        /// <summary>
        ///     更新日 (降順)
        /// </summary>
        UpdatedAtDesc,

        /// <summary>
        ///     サイズ (昇順)
        /// </summary>
        SizeAsc,

        /// <summary>
        ///     サイズ (降順)
        /// </summary>
        SizeDesc
    }
}