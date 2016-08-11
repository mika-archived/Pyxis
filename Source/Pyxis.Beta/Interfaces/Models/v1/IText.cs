using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IText : IErrorResponse
    {
        INovelMarker NovelMarker { get; }

        string NovelText { get; }

        INovel SeriesPrev { get; }

        INovel SeriesNext { get; }
    }
}