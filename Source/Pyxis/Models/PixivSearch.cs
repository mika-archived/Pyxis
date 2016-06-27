﻿using System;
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
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Search(string query, SearchOptionParameter optionParameter)
        {
            ResultIllusts.Clear();
            ResultNovels.Clear();
            ResultUsers.Clear();
            _query = query;
            _optionParam = optionParameter;
#if !OFFLINE
            RunHelper.RunLaterAsync(SearchAsync, TimeSpan.FromMilliseconds(500));
#endif
        }

        private async Task SearchAsync()
        {
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
            IIllusts illusts;
            if (_optionParam.Duration == SearchDuration.Nothing)
                illusts = await _pixivClient.Search.IllustAsync(search_target => _optionParam.Target.ToParamString(),
                                                                sort => _optionParam.Sort.ToParamString(),
                                                                word => _query,
                                                                filter => "for_ios",
                                                                offset => Count());
            else
                illusts = await _pixivClient.Search.IllustAsync(duration => _optionParam.Duration.ToParamString(),
                                                                search_target => _optionParam.Target.ToParamString(),
                                                                sort => _optionParam.Sort.ToParamString(),
                                                                word => _query,
                                                                filter => "for_ios",
                                                                offset => Count());
            illusts?.IllustList.ForEach(w => ResultIllusts.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SearchNovel()
        {
            INovels novels;
            if (_optionParam.Duration == SearchDuration.Nothing)
                novels = await _pixivClient.Search.NovelAsync(search_target => _optionParam.Target.ToParamString(),
                                                              sort => _optionParam.Sort.ToParamString(),
                                                              word => _query,
                                                              offset => Count());
            else
                novels = await _pixivClient.Search.NovelAsync(duration => _optionParam.Duration.ToParamString(),
                                                              search_target => _optionParam.Target.ToParamString(),
                                                              sort => _optionParam.Sort.ToParamString(),
                                                              word => _query,
                                                              offset => Count());

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
            if (_optionParam.SearchType == SearchType.IllustsAndManga)
                return ResultIllusts.Count;
            if (_optionParam.SearchType == SearchType.Novels)
                return ResultNovels.Count;
            if (_optionParam.SearchType == SearchType.Users)
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