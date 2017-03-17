using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivBlocking : ISupportIncrementalLoading
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private int _offset;
        public ObservableCollection<User> Users { get; }

        public PixivBlocking(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            Users = new ObservableCollection<User>();
            _offset = 0;
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task Fetch()
        {
            var users = await _pixivClient.User.ListAsync(offset: _offset);
            users?.UserPreviews.ForEach(w => Users.Add(w.User));
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
                await Fetch();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}