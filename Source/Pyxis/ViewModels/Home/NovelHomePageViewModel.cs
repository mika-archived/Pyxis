using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Items;
using Pyxis.ViewModels.Items;

using Reactive.Bindings;

namespace Pyxis.ViewModels.Home
{
    public class NovelHomePageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private readonly PixivRanking _pixivRanking;
        private readonly PixivRecommended _pixivRecommended;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveCollection<RankingNovelViewModel> TopTankingImages { get; private set; }
        public ReadOnlyReactiveCollection<PixivNovelViewModel> RecommendedNovels { get; private set; }

        public NovelHomePageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                      INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            _pixivRanking = new PixivRanking(pixivClient, ContentType.Novel);
            _pixivRecommended = new PixivRecommended(pixivClient, ContentType.Novel);

            TopTankingImages = _pixivRanking.RankingOfNovels
                                            .ToReadOnlyReactiveCollection(CreateRankingNovel)
                                            .AddTo(this);
            RecommendedNovels = _pixivRecommended.RecommendedNovels
                                                 .ToReadOnlyReactiveCollection(CreatePixivNovel)
                                                 .AddTo(this);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _pixivRanking.Fetch();
        }

        #endregion

        private RankingNovelViewModel CreateRankingNovel(Tuple<RankingMode, INovels> w) =>
            new RankingNovelViewModel(w.Item1, w.Item2.NovelList.First(), _imageStoreService, NavigationService);

        private PixivNovelViewModel CreatePixivNovel(INovel w) =>
            new PixivNovelViewModel(w, _imageStoreService, NavigationService);
    }
}