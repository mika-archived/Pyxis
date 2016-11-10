using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivNovelText : BindableBase
    {
        private readonly INovel _novel;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        public PixivNovelText(INovel novel, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _novel = novel;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
        }

        public void Fetch() => RunHelper.RunAsync(FetchText);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchText() => Text = await _queryCacheService.RunAsync(_pixivClient.NovelV1.TextAsync, novel_id => _novel.Id);

        #region Text

        private IText _text;

        public IText Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion
    }
}