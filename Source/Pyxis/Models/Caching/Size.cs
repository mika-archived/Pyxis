namespace Pyxis.Models.Caching
{
    // KB
    internal enum Size
    {
        /// <summary>
        ///     500MB まで
        /// </summary>
        FiveHundredMegabytes = 500000,

        /// <summary>
        ///     1GB まで
        /// </summary>
        OneGigabyte = 1000000,

        /// <summary>
        ///     2GB まで
        /// </summary>
        TwoGigabyte = 2000000,

        /// <summary>
        ///     5GB まで
        /// </summary>
        FiveGigabyte = 5000000,

        /// <summary>
        ///     無制限 (2TB)
        /// </summary>
        Unlimited = int.MaxValue
    }
}