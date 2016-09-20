using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Services.Interfaces
{
    public interface IBrowsingHistoryService
    {
        void Add(IIllust illust);

        void Add(INovel novel);

        void ForcePush();
    }
}