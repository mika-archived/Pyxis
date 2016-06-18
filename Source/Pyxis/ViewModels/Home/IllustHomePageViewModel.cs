using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home.Items;

using Reactive.Bindings;

namespace Pyxis.ViewModels.Home
{
    public class IllustHomePageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private readonly PixivRanking _pixivRanking;
        public INavigationService NavigationService { get; }

        #region Lambda Functions

        private Func<Tuple<RankingMode, IIllusts>, RankingImageViewModel> Func01 =>
            w => new RankingImageViewModel(w.Item1, w.Item2.IllustList.First(), _imageStoreService, NavigationService);

        #endregion

        public ReadOnlyReactiveCollection<RankingImageViewModel> TopRankingImages { get; private set; }

        public IllustHomePageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                       INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            _pixivRanking = new PixivRanking(pixivClient, RankingType.Illust);

            TopRankingImages = _pixivRanking.Ranking
                                            .ToReadOnlyReactiveCollection(w => Func01(w))
                                            .AddTo(this);
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            _pixivRanking.Fetch();
        }

        #endregion
    }
}