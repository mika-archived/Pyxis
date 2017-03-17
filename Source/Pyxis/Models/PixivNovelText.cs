using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivNovelText : BindableBase
    {
        private readonly Novel _novel;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        public PixivNovelText(Novel novel, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _novel = novel;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
        }

        public void Fetch() => RunHelper.RunAsync(FetchText);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchText() => Text = await _pixivClient.Novel.TextAsync(_novel.Id);

        #region Text

        private Text _text;

        public Text Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion
    }
}