using System;

namespace Pyxis.Models.Cache
{
    public class CacheFile
    {
        /// <summary>
        ///     ファイルの固有 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     ファイルサイズ
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        ///     ファイルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     最終参照日時
        /// </summary>
        public DateTime ReferencedAt { get; set; }
    }
}