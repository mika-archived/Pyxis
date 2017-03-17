using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivTrending
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly SearchType _searchType;

        public ObservableCollection<TrendingTag> TrendingTags { get; }

        public PixivTrending(SearchType searchType, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _searchType = searchType;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            TrendingTags = new ObservableCollection<TrendingTag>();
        }

        public void Fetch() => RunHelper.RunAsync(FetchTrendingTags);

        private async Task FetchTrendingTags()
        {
            TrendingTags trendingTags;
            if (_searchType == SearchType.IllustsAndManga)
                trendingTags = await _pixivClient.TrendingTags.IllustAsync("for_ios");
            else if (_searchType == SearchType.Novels)
                trendingTags = await _pixivClient.TrendingTags.NovelAsync("for_ios");
            else
                throw new NotSupportedException();
            trendingTags?.Tags.ForEach(w => TrendingTags.Add(w));
        }
    }
}