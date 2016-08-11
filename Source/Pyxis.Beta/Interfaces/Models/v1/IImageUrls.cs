namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IImageUrls
    {
        /// <summary>
        ///     360x360
        /// </summary>
        string SquareMedium { get; }

        /// <summary>
        ///     540x540
        /// </summary>
        string Medium { get; }

        /// <summary>
        ///     600x1200
        /// </summary>
        string Large { get; }

        /// <summary>
        ///     原寸
        /// </summary>
        string Original { get; }

        /// <summary>
        ///     Equals to Original
        /// </summary>
        string OriginalImageUrl { get; }
    }
}