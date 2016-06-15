namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IText
    {
        INovelMarker NovelMarker { get; }

        string NovelText { get; }

        INovel SeriesPrev { get; }

        INovel SeriesNext { get; }
    }
}