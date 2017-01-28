using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivMypixiv : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly string _userId;
        private string _offset;
        public ObservableCollection<IUserPreview> Users { get; }

        public PixivMypixiv(string userId, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _userId = userId;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            Users = new ObservableCollection<IUserPreview>();
            _offset = "";
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task Fetch()
        {
            var users = await _queryCacheService.RunAsync(_pixivClient.UserV1.MypixivAsync,
                                                          user_id => _userId,
                                                          offset => _offset);
            users?.UserPreviewList.ForEach(w => Users.Add(w));
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
                await Fetch();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}