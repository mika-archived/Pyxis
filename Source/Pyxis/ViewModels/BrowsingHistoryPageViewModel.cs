using System;
using System.Reactive.Linq;

using Microsoft.Toolkit.Uwp;

using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels
{
    public class BrowsingHistoryPageViewModel : ViewModel
    {
        // public ReactiveProperty<int> SelectedIndex { get; }
        public ReactiveProperty<object> SelectedItem { get; }

        public IncrementalLoadingCollection<PixivBrowsingHistorySource<Illust, IllustViewModel>, IllustViewModel> IllustSource { get; }
        public IncrementalLoadingCollection<PixivBrowsingHistorySource<Novel, NovelViewModel>, NovelViewModel> NovelSource { get; }

        public BrowsingHistoryPageViewModel(PixivClient pixivClient)
        {
            IllustSource = new IncrementalLoadingCollection<PixivBrowsingHistorySource<Illust, IllustViewModel>, IllustViewModel>(
                new PixivBrowsingHistorySource<Illust, IllustViewModel>(pixivClient, w => new IllustViewModel(w)));
            NovelSource = new IncrementalLoadingCollection<PixivBrowsingHistorySource<Novel, NovelViewModel>, NovelViewModel>(
                new PixivBrowsingHistorySource<Novel, NovelViewModel>(pixivClient, w => new NovelViewModel(w)));

            /*
            SelectedIndex = new ReactiveProperty<int>();
            SelectedIndex.SelectMany(async w =>
            {
                switch (w)
                {
                    case 0:
                        // await pixivRanking.FetchIllustRankingAsync(RankingMode.Day, null);
                        break;

                    case 1:
                        // await pixivRanking.FetchMangaRankingAsync(RankingMode.DayManga, null);
                        break;

                    default:
                        throw new NotSupportedException("Wrong Index Number of Pivot.");
                }
                return Unit.Default;
            }).Subscribe().AddTo(this);
            */
            SelectedItem = new ReactiveProperty<object>();
            SelectedItem.Select(w => w as ContentViewModel).Where(w => w != null)
                        .Subscribe(w => w.NavigateTo()).AddTo(this);
        }
    }
}