using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Items;
using Pyxis.ViewModels.Items;

using Reactive.Bindings;

namespace Pyxis.ViewModels.Home
{
    public class IllustHomePageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private readonly PixivRanking _pixivRanking;
        private readonly PixivRecommended _pixivRecommended;
        public INavigationService NavigationService { get; }

        public ReadOnlyReactiveCollection<RankingImageViewModel> TopRankingImages { get; private set; }
        public IncrementalObservableCollection<PixivImageViewModel> RecommendedImages { get; }

        public IllustHomePageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                       INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            _pixivRanking = new PixivRanking(pixivClient, ContentType.Illust);
            _pixivRecommended = new PixivRecommended(_pixivClient, ContentType.Illust);

            TopRankingImages = _pixivRanking.Ranking
                                            .ToReadOnlyReactiveCollection(CreateRankingImage)
                                            .AddTo(this);

            // RecommendedImages = new IncrementalObservableCollection<PixivImageViewModel>();
            // ModelHelper.ConnectTo(RecommendedImages, _pixivRecommended, w => w.RecommendedImages, CreatePixivImage);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _pixivRanking.Fetch();
        }

        #endregion

        private RankingImageViewModel CreateRankingImage(Tuple<RankingMode, IIllusts> w) =>
            new RankingImageViewModel(w.Item1, w.Item2.IllustList.First(), _imageStoreService, NavigationService);

        private PixivImageViewModel CreatePixivImage(IIllust w) =>
            new PixivImageViewModel(w, _imageStoreService, NavigationService);
    }
}