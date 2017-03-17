using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

using SearchTarget = Sagitta.Enum.SearchTarget;

namespace Pyxis.Models
{
    internal class PixivSearch : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _count;
        private int _offset;
        private SearchOptionParameter _optionParam;

        private string _query;

        public ObservableCollection<Illust> ResultIllustsRoot { get; }
        public ObservableCollection<Novel> ResultNovels { get; }
        public ObservableCollection<UserPreview> ResultUsers { get; }

        public PixivSearch(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            ResultIllustsRoot = new ObservableCollection<Illust>();
            ResultNovels = new ObservableCollection<Novel>();
            ResultUsers = new ObservableCollection<UserPreview>();
            _offset = 0;
            _count = 0;
            HasMoreItems = false;
        }

        public void Search(string query, SearchOptionParameter optionParameter, bool force = false)
        {
            ResultIllustsRoot.Clear();
            ResultNovels.Clear();
            ResultUsers.Clear();
            _query = query;
            _offset = 0;
            _count = 0;
            _optionParam = optionParameter;
            if (!string.IsNullOrWhiteSpace(_optionParam.EitherWord))
                _query += " " + string.Join(" ", _optionParam.EitherWord.Split(' ').Select(w => $"({w})"));
            if (!string.IsNullOrWhiteSpace(_optionParam.IgnoreWord))
                _query += " " + string.Join(" ", _optionParam.IgnoreWord.Split(' ').Select(w => $"--{w}"));
#if !OFFLINE
            HasMoreItems = true;
            if (force)
                RunHelper.RunAsync(SearchAsync);
#endif
        }

        private async Task SearchAsync()
        {
            if (_optionParam == null || string.IsNullOrWhiteSpace(_query))
            {
                HasMoreItems = false;
                return;
            }
            if (_count > 0 && _offset == 0)
                _offset = 30; // クソ
            _count++;
            if (_optionParam.SearchType == SearchType.IllustsAndManga)
                await SearchIllust();
            else if (_optionParam.SearchType == SearchType.Novels)
                await SearchNovel();
            else if (_optionParam.SearchType == SearchType.Users)
                await SearchUser();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SearchIllust()
        {
            var illusts = await _pixivClient.Search.IllustAsync(_query, SearchTarget.ExactMatchForTags, SortOrder.PopularDesc, filter: "for_ios",
                                                                offset: _offset);
            illusts?.Illusts.Where(w => w.TotalBookmarks >= _optionParam.BookmarkCount)
                    .Where(w => w.TotalView >= _optionParam.ViewCount)
                    .Where(w => w.TotalView >= _optionParam.CommentCount)
                    .Where(w => w.PageCount >= _optionParam.CommentCount)
                    .Where(w => w.Height >= _optionParam.Height && w.Width >= _optionParam.Width)
                    .Where(w => string.IsNullOrWhiteSpace(_optionParam.Tool) || w.Tools.Any(v => v == _optionParam.Tool))
                    .ForEach(w => ResultIllustsRoot.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(illusts.NextUrl)["offset"]);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SearchNovel()
        {
            var novels = await _pixivClient.Search.NovelAsync(_query, SearchTarget.ExactMatchForTags, SortOrder.PopularDesc, offset: _offset);
            novels?.Novels.Where(w => w.TotalBookmarks >= _optionParam.BookmarkCount)
                   .Where(w => w.TotalView >= _optionParam.ViewCount)
                   .Where(w => w.TotalComments >= _optionParam.CommentCount)
                   .Where(w => w.PageCount >= _optionParam.CommentCount)
                   .Where(w => w.TextLength >= _optionParam.TextLength)
                   .ForEach(w => ResultNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(novels.NextUrl)["offset"]);
        }

        private async Task SearchUser()
        {
            var users = await _pixivClient.Search.UserAsync(_query, _offset);
            users?.UserPreviews.ForEach(w => ResultUsers.Add(w));
            if (string.IsNullOrWhiteSpace(users?.NextUrl))
                HasMoreItems = false;
            else
                _offset = int.Parse(UrlParameter.ParseQuery(users.NextUrl)["offset"]);
        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await SearchAsync();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}