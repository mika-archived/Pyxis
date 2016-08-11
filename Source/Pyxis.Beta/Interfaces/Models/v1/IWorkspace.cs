namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     ユーザー作業環境
    /// </summary>
    public interface IWorkspace
    {
        /// <summary>
        ///     コンピューター
        /// </summary>
        string Pc { get; }

        /// <summary>
        ///     モニター
        /// </summary>
        string Monitor { get; }

        /// <summary>
        ///     ソフト
        /// </summary>
        string Tool { get; }

        /// <summary>
        ///     スキャナー
        /// </summary>
        string Scanner { get; }

        /// <summary>
        ///     タブレット
        /// </summary>
        string Tablet { get; }

        /// <summary>
        ///     マウス
        /// </summary>
        string Mouse { get; }

        /// <summary>
        ///     プリンター
        /// </summary>
        string Printer { get; }

        /// <summary>
        ///     机の上にあるもの
        /// </summary>
        string Desktop { get; }

        /// <summary>
        ///     絵を描く時に聞く音楽
        /// </summary>
        string Music { get; }

        /// <summary>
        ///     机
        /// </summary>
        string Desk { get; }

        /// <summary>
        ///     椅子
        /// </summary>
        string Chair { get; }

        /// <summary>
        ///     コメント
        /// </summary>
        string Comment { get; }

        /// <summary>
        ///     作業環境画像
        /// </summary>
        string WorkspaceImageUrl { get; }
    }
}