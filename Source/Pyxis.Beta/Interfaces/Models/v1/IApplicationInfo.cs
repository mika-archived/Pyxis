using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     アプリケーション情報
    ///     ** API only
    /// </summary>
    public interface IApplicationInfo : IErrorResponse
    {
        string LatestVersion { get; }

        bool UpdateRequired { get; }

        bool UpdateAvailable { get; }

        string UpdateMessage { get; }

        bool NoticeExists { get; }

        string StoreUrl { get; }

        string NoticeId { get; }

        bool NoticeImportant { get; }

        string NoticeMessage { get; }
    }
}