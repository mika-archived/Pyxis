using Pyxis.Beta.Interfaces.Rest.v1;

namespace Pyxis.Beta.Interfaces.Rest.v2
{
    public interface IUserBrowsingHistoryApi
    {
        IUserBrowsingHistoryIllustApi Illust { get; }
    }
}