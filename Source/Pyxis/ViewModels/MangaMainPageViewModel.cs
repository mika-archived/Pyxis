using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;

namespace Pyxis.ViewModels
{
    public class MangaMainPageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly INavigationService _navigationService;
        private readonly IPixivClient _pixivClient;
        private readonly PixivRanking _pixivRanking;

        #region Lambda Functions

        private Func<IIllusts, PixivImageViewModel> Func01
            => w => new PixivImageViewModel(w.IllustList.First(), _imageStoreService, _navigationService);

        #endregion

        public ReadOnlyReactiveCollection<PixivImageViewModel> TopTankingImages { get; private set; }

        public MangaMainPageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                      INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            _navigationService = navigationService;
            _pixivRanking = new PixivRanking(pixivClient, RankingType.Manga);

            TopTankingImages = _pixivRanking.Ranking
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