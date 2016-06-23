using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Microsoft.Practices.ObjectBuilder2;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models.Enums;

namespace Pyxis.Models
{
    internal class PixivNew : ISupportIncrementalLoading
    {
        private readonly ContentType _contentType;
        private readonly FollowType _followType;
        private readonly IPixivClient _pixivClient;

        public ObservableCollection<IIllust> NewIllusts { get; }
        public ObservableCollection<INovel> NewNovels { get; }

        public PixivNew(ContentType contentType, FollowType followType, IPixivClient pixivClient)
        {
            _contentType = contentType;
            _followType = followType;
            _pixivClient = pixivClient;
            HasMoreItems = false;
            NewIllusts = new ObservableCollection<IIllust>();
            NewNovels = new ObservableCollection<INovel>();
        }

        private async Task FetchNewItems(bool isClear)
        {
            if (_contentType == ContentType.Novel)
                await FetchNovels(isClear);
            else
                await FetchIllusts(isClear);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchNovels(bool isClear)
        {
            if (isClear)
                NewNovels.Clear();

            INovels novels = null;
            if (_followType == FollowType.Following)
                novels = await _pixivClient.NovelV1.FollowAsync(restrict => "all", offset => Count());
            else if (_followType == FollowType.Mypixiv)
                novels = await _pixivClient.NovelV1.MypixivAsync(offset => Count());
            else if (_followType == FollowType.All)
                novels = await _pixivClient.NovelV1.NewAsync(max_novel_id => Count() > 0 ? NewNovels.Last().Id : 0);
            novels?.NovelList.ForEach(w => NewNovels.Add(w));
            if (novels?.NovelList.Count == 0)
                HasMoreItems = false;
        }

        private int Count()
        {
            if (_contentType == ContentType.Novel)
                return NewIllusts.Count;
            return NewNovels.Count;
        }

        #region Illust related

        private async Task FetchIllusts(bool isClear)
        {
            if (isClear)
                NewIllusts.Clear();

            IIllusts illusts;
            if (_contentType == ContentType.Illust)
                illusts = await FetchIllusts();
            else
                illusts = await FetchManga();
            illusts?.IllustList.ForEach(w => NewIllusts.Add(w));
            if (illusts?.IllustList.Count == 0)
                HasMoreItems = false;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task<IIllusts> FetchIllusts()
        {
            IIllusts illusts = null;
            if (_followType == FollowType.Following)
                illusts = await _pixivClient.IllustV2.FollowAsync(restrict => "all", offset => Count());
            else if (_followType == FollowType.Mypixiv)
                illusts = await _pixivClient.IllustV2.MypixivAsync(offset => Count());
            else if (_followType == FollowType.All)
                illusts = await _pixivClient.IllustV1.NewAsync(filter => "for_ios", content_type => "illust",
                                                               offset => Count());
            return illusts;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task<IIllusts> FetchManga()
        {
            IIllusts illusts = null;
            if (_followType == FollowType.Following)
                throw new NotSupportedException("Following");
            if (_followType == FollowType.Mypixiv)
                throw new NotSupportedException("Mypixiv");
            if (_followType == FollowType.All)
                illusts = await _pixivClient.IllustV1.NewAsync(filter => "for_ios", content_type => "manga",
                                                               offset => Count());
            return illusts;
        }

        #endregion

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await FetchNewItems(false);
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}