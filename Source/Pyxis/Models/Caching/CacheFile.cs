using System;

namespace Pyxis.Models.Caching
{
    public class CacheFile
    {
        public int Id { get; set; }

        /// <summary>
        ///     キャッシュカテゴリ
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        ///     ファイルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     ファイルサイズ (bytes)
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        ///     作成日
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     更新日
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}