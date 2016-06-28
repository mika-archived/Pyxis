using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;

namespace Pyxis.Models
{
    internal class PixivFavorite : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;

        private string _maxBookmarkId;
        private FavoriteOptionParameter _optionParam;

        public ObservableCollection<IIllust> ResultIllusts { get; }
        public ObservableCollection<INovel> ResultNovels { get; }

        public PixivFavorite(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            ResultIllusts = new ObservableCollection<IIllust>();
            ResultNovels = new ObservableCollection<INovel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Query(FavoriteOptionParameter optionParameter)
        {
            ResultIllusts.Clear();
            ResultNovels.Clear();
            _optionParam = optionParameter;
            _maxBookmarkId = "";
#if !OFFLINE
            HasMoreItems = true;
#endif
        }

        private async Task QueryAsync()
        {
            if (_optionParam == null)
            {
                HasMoreItems = false;
                return;
            }
            if (_optionParam.Type == SearchType.IllustsAndManga)
                await QueryIllust();
            else if (_optionParam.Type == SearchType.Novels)
                await QueryNovel();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task QueryIllust()
        {
            var bookmarksApi = _pixivClient.User.Bookmarks;
            var illusts = await bookmarksApi.IllustAsync(user_id => _optionParam.UserId,
                                                         filter => "for_ios",
                                                         restrict => _optionParam.Restrict.ToParamString(),
                                                         max_bookmark_id => _maxBookmarkId,
                                                         tag => _optionParam.Tag ?? "");
            illusts?.IllustList.ForEach(w => ResultIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _maxBookmarkId = UrlParameter.ParseQuery(illusts.NextUrl)["max_bookmark_id"];
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task QueryNovel()
        {
            var bookmarksApi = _pixivClient.User.Bookmarks;
            var novels = await bookmarksApi.NovelAsync(user_id => _optionParam.UserId,
                                                       restrict => _optionParam.Restrict.ToParamString(),
                                                       max_bookmark_id => _maxBookmarkId,
                                                       tag => _optionParam.Tag ?? "");
            novels?.NovelList.ForEach(w => ResultNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _maxBookmarkId = UrlParameter.ParseQuery(novels.NextUrl)["max_bookmark_id"];
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await QueryAsync();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}