using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    public class NovelApi : INovelApi
    {
        private readonly PixivApiClient _client;

        public NovelApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of INovelApi

        public INovelBookmarkApi Bookmark => new NovelBookmarkApi(_client);

        #endregion
    }
}