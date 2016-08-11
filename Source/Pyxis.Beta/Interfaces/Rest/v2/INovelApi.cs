namespace Pyxis.Beta.Interfaces.Rest.v2
{
    public interface INovelApi
    {
        INovelBookmarkApi Bookmark { get; }
    }
}