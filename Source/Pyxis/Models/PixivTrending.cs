using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivTrending
    {
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly SearchType _searchType;

        public ObservableCollection<ITrendTag> TrendingTags { get; }

        public PixivTrending(SearchType searchType, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _searchType = searchType;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            TrendingTags = new ObservableCollection<ITrendTag>();
        }

        public void Fetch() => RunHelper.RunAsync(FetchTrendingTags);

        private async Task FetchTrendingTags()
        {
            ITrendingTags trendingTags;
            if (_searchType == SearchType.IllustsAndManga)
                trendingTags = await _queryCacheService.RunAsync(_pixivClient.TrendingTags.IllustAsync, filter => "for_ios");
            else if (_searchType == SearchType.Novels)
                trendingTags = await _queryCacheService.RunAsync(_pixivClient.TrendingTags.NovelAsync, filter => "for_ios");
            else
                throw new NotSupportedException();
            trendingTags?.TrendTags.ForEach(w => TrendingTags.Add(w));
        }
    }
}