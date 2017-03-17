using Sagitta.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IBrowsingHistoryService
    {
        void Add(Illust illust);

        void Add(Novel novel);

        void ForcePush();
    }
}