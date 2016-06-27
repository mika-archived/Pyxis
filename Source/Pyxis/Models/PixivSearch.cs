using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;

namespace Pyxis.Models
{
    internal class PixivSearch : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly SearchType _searchType;

        private string _query;

        public ObservableCollection<IIllust> ResultIllusts { get; }
        public ObservableCollection<INovel> ResultNovels { get; }
        public ObservableCollection<IUserPreview> ResultUsers { get; }

        public PixivSearch(IPixivClient pixivClient, SearchType searchType)
        {
            _pixivClient = pixivClient;
            _searchType = searchType;
            ResultIllusts = new ObservableCollection<IIllust>();
            ResultNovels = new ObservableCollection<INovel>();
            ResultUsers = new ObservableCollection<IUserPreview>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Search(string query)
        {
            ResultIllusts.Clear();
            ResultNovels.Clear();
            ResultUsers.Clear();
            _query = query;
#if !OFFLINE
            RunHelper.RunLaterAsync(SearchAsync, TimeSpan.FromMilliseconds(500));
#endif
        }

        private async Task SearchAsync()
        {
            if (_searchType == SearchType.IllustsAndManga)
                await SearchIllust();
            else if (_searchType == SearchType.Novels)
                await SearchNovel();
            else if (_searchType == SearchType.Users)
                await SearchUser();
            else
                throw new NotSupportedException();
        }

        private async Task SearchIllust()
        {
            var illusts = await _pixivClient.Search.IllustAsync(word => _query, filter => "for_ios", offset => Count());
            illusts?.IllustList.ForEach(w => ResultIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
        }

        private async Task SearchNovel()
        {
            var novels = await _pixivClient.Search.NovelAsync(word => _query, offset => Count());
            novels?.NovelList.ForEach(w => ResultNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
        }

        private async Task SearchUser()
        {
            var users = await _pixivClient.Search.UserAsync(word => _query, filter => "for_ios", offset => Count());
            users?.UserPreviewList.ForEach(w => ResultUsers.Add(w));
            if (string.IsNullOrWhiteSpace(users?.NextUrl))
                HasMoreItems = false;
        }

        private int Count()
        {
            if (_searchType == SearchType.IllustsAndManga)
                return ResultIllusts.Count;
            if (_searchType == SearchType.Novels)
                return ResultNovels.Count;
            if (_searchType == SearchType.Users)
                return ResultUsers.Count;
            throw new NotSupportedException();
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