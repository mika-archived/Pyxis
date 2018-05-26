using System;

namespace Pyxis.Models
{
    public class ObjectCache
    {
        /// <summary>
        ///     有効期限
        /// </summary>
        public DateTime ExpiredAt { get; set; }

        /// <summary>
        ///     オブジェクト
        /// </summary>
        public object Value { get; set; }
    }
}