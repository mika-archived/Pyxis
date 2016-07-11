using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;

namespace Pyxis.Models
{
    internal class PixivFollower : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private readonly string _userId;
        private string _offset;

        public ObservableCollection<IUserPreview> Users { get; }

        public PixivFollower(string userId, IPixivClient pixivClient)
        {
            _userId = userId;
            _pixivClient = pixivClient;
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
            var users = await _pixivClient.User.FollowerAsync(user_id => _userId, offset => _offset);
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