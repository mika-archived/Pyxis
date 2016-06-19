using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Items;
using Pyxis.ViewModels.Items;

using Reactive.Bindings;

namespace Pyxis.ViewModels.Home
{
    public class MangaHomePageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private readonly PixivRanking _pixivRanking;
        private readonly PixivRecommended _pixivRecommended;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveCollection<RankingImageViewModel> TopTankingImages { get; private set; }
        public ReadOnlyReactiveCollection<PixivImageViewModel> RecommendedImages { get; private set; }

        public MangaHomePageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                      INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            _pixivRanking = new PixivRanking(pixivClient, ContentType.Manga);
            _pixivRecommended = new PixivRecommended(pixivClient, ContentType.Manga);

            TopTankingImages = _pixivRanking.Ranking
                                            .ToReadOnlyReactiveCollection(CreateRankingImage)
                                            .AddTo(this);
            RecommendedImages = _pixivRecommended.RecommendedImages
                                                 .ToReadOnlyReactiveCollection(CreatePixivImage)
                                                 .AddTo(this);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _pixivRanking.Fetch();
            RunHelper.RunLater(_pixivRecommended.Fetch, false, TimeSpan.FromMilliseconds(500));
        }

        #endregion

        private RankingImageViewModel CreateRankingImage(Tuple<RankingMode, IIllusts> w) =>
            new RankingImageViewModel(w.Item1, w.Item2.IllustList.First(), _imageStoreService, NavigationService);

        private PixivImageViewModel CreatePixivImage(IIllust w) =>
            new PixivImageViewModel(w, _imageStoreService, NavigationService);
    }
}