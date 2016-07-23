using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;

namespace Pyxis.Models
{
    internal class PixivSearch : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private string _offset;
        private SearchOptionParameter _optionParam;

        private string _query;

        public ObservableCollection<IIllust> ResultIllusts { get; }
        public ObservableCollection<INovel> ResultNovels { get; }
        public ObservableCollection<IUserPreview> ResultUsers { get; }

        public PixivSearch(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            ResultIllusts = new ObservableCollection<IIllust>();
            ResultNovels = new ObservableCollection<INovel>();
            ResultUsers = new ObservableCollection<IUserPreview>();
            _offset = "";
            HasMoreItems = false;
        }

        public void Search(string query, SearchOptionParameter optionParameter)
        {
            var flag = ResultIllusts.Count == 0 && ResultNovels.Count == 0 && ResultUsers.Count == 0;
            ResultIllusts.Clear();
            ResultNovels.Clear();
            ResultUsers.Clear();
            _query = query;
            _offset = "";
            _optionParam = optionParameter;
#if !OFFLINE
            HasMoreItems = true;
            if (flag)
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
            var illusts = await _pixivClient.Search.IllustAsync(duration => _optionParam.Duration.ToParamString(),
                                                                search_target => _optionParam.Target.ToParamString(),
                                                                sort => _optionParam.Sort.ToParamString(),
                                                                word => _query,
                                                                filter => "for_ios",
                                                                offset => _offset);
            illusts?.IllustList.ForEach(w => ResultIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SearchNovel()
        {
            var novels = await _pixivClient.Search.NovelAsync(duration => _optionParam.Duration.ToParamString(),
                                                              search_target => _optionParam.Target.ToParamString(),
                                                              sort => _optionParam.Sort.ToParamString(),
                                                              word => _query,
                                                              offset => _offset);

            novels?.NovelList.ForEach(w => ResultNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

        private async Task SearchUser()
        {
            var users = await _pixivClient.Search.UserAsync(word => _query, filter => "for_ios", offset => _offset);
            users?.UserPreviewList.ForEach(w => ResultUsers.Add(w));
            if (string.IsNullOrWhiteSpace(users?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(users.NextUrl)["offset"];
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