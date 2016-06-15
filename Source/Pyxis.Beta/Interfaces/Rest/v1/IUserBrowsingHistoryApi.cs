namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserBrowsingHistoryApi
    {
        IUserBrowsingHistoryIllustApi Illust { get; }

        IUserBrowsingHistoryNovelApi Novel { get; }
    }
}