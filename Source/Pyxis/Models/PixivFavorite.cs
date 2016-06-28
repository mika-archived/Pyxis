using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;

namespace Pyxis.Models
{
    internal class PixivFavorite : ISupportIncrementalLoading
    {
        private readonly IPixivClient _pixivClient;
        private FavoriteOptionParameter _optionParam;

        public ObservableCollection<IIllust> ResultIllusts { get; }
        public ObservableCollection<INovel> ResultNovels { get; }

        public PixivFavorite(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            ResultIllusts = new ObservableCollection<IIllust>();
            ResultNovels = new ObservableCollection<INovel>();
#if OFFLINE
            HasMoreItems = false;
#else
            HasMoreItems = true;
#endif
        }

        public void Query(FavoriteOptionParameter optionParameter)
        {
            ResultIllusts.Clear();
            ResultNovels.Clear();
            _optionParam = optionParameter;
#if !OFFLINE
            HasMoreItems = true;
            RunHelper.RunLaterAsync(QueryAsync, TimeSpan.FromMilliseconds(500));
#endif
        }

        private async Task QueryAsync()
        {
            if (_optionParam.Type == SearchType.IllustsAndManga)
                await QueryIllust();
            else if (_optionParam.Type == SearchType.Novels)
                await QueryNovel();
            else
                throw new NotSupportedException();
        }

        private async Task QueryIllust()
        {

        }

        private async Task QueryNovel()
        {

        }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                await QueryAsync();
                return new LoadMoreItemsResult {Count = 30};
            }).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        #endregion
    }
}