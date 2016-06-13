namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     アプリケーション情報
    ///     ** API only
    /// </summary>
    public interface IApplicationInfo
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