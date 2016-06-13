namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface ITrendTag
    {
        /// <summary>
        ///     タグ
        /// </summary>
        string Tag { get; }

        /// <summary>
        ///     トップイラスト
        /// </summary>
        IIllust Illust { get; }
    }
}