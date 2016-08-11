namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     プロフィール画像
    /// </summary>
    public interface IProfileImageUrls
    {
        string Size16 { get; }

        string Size50 { get; }

        string Size170 { get; }

        /// <summary>
        ///     Equals to Size170
        /// </summary>
        string Medium { get; }
    }
}