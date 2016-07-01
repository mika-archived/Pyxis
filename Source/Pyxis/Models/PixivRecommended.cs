using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

// ReSharper disable InconsistentNaming

namespace Pyxis.Models
{
    internal class PixivRecommended : ISupportIncrementalLoading
    {
        private readonly IAccountService _accountService;
        private readonly ContentType _contentType;
        private readonly IPixivClient _pixivClient;
        private string _offset;

        public ObservableCollection<IIllust> RecommendedImages { get; }
        public ObservableCollection<INovel> RecommendedNovels { get; }
        public ObservableCollection<IUserPreview> RecommendedUsers { get; }

        public PixivRecommended(IAccountService accountService, IPixivClient pixivClient, ContentType contentType)
        {
            _accountService = accountService;
            _contentType = contentType;
            _pixivClient = pixivClient;
            _offset = "";
            RecommendedNovels = new ObservableCollection<INovel>();
            RecommendedImages = new ObservableCollection<IIllust>();
            RecommendedUsers = new ObservableCollection<IUserPreview>();
#if OFFLINE
            HasMoreItems = false;
            if (_contentType == ContentType.User)
                HasMoreItems = true;
#else
            HasMoreItems = true;
#endif
        }

        private async Task FetchRecommended()
        {
            if (_contentType == ContentType.Novel)
                await FetchNovels();
            else if (_contentType == ContentType.User)
                await FetchUsers();
            else if (_contentType == ContentType.Illust || _contentType == ContentType.Manga)
                await FetchIllusts();
            else
                throw new NotSupportedException();
        }

        private async Task FetchIllusts()
        {
            IRecommendedIllusts illusts = null;
            if (_contentType == ContentType.Illust)
                illusts = await RecommendedAsync("illust");
            else if (_contentType == ContentType.Manga)
                illusts = await RecommendedAsync("manga");
            illusts?.Illusts.ForEach(w => RecommendedImages.Add(w));
            if (string.IsNullOrWhiteSpace(illusts?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(illusts.NextUrl)["offset"];
        }

        private async Task<IRecommendedIllusts> RecommendedAsync(string contentType)
        {
            // manga/recommended との違いがわからない。
            if (_accountService.IsLoggedIn)
                return await _pixivClient.IllustV1.RecommendedAsync(content_type => contentType,
                                                                    filter => "for_ios",
                                                                    offset => _offset);
            return await _pixivClient.IllustV1.RecommendedNologinAsync(content_type => contentType,
                                                                       filter => "for_ios",
                                                                       offset => _offset);
        }

        private async Task FetchNovels()
        {
            IRecommendedNovels novels;
            if (_accountService.IsLoggedIn)
                novels = await _pixivClient.NovelV1.RecommendedAsync(offset => _offset);
            else
                novels = await _pixivClient.NovelV1.RecommendedNologinAsync(offset => _offset);
            novels?.Novels.ForEach(w => RecommendedNovels.Add(w));
            if (string.IsNullOrWhiteSpace(novels?.NextUrl))
                HasMoreItems = false;
            else
                _offset = UrlParameter.ParseQuery(novels.NextUrl)["offset"];
        }

        private async Task FetchUsers()
        {
            var users = await _pixivClient.User.RecommendedAsync(offset => _offset);
            users?.UserPreviewList.ForEach(w => RecommendedUsers.Add(w));
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
                await FetchRecommended();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}