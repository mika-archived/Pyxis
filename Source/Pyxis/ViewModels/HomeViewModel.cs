using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Models;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Home;

using Reactive.Bindings;

using Sagitta;

namespace Pyxis.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ITitleService _titleService;
        public IllustContentViewModel IllustContentViewModel { get; }
        public MangaContentViewModel MangaContentViewModel { get; }
        public ReactiveProperty<int> SelectedTab { get; }

        public HomeViewModel(PixivClient pixivClient, INavigationService navigationService, IObjectCacheStorage objectCacheStorage, ITitleService titleService)
        {
            _navigationService = navigationService;
            _titleService = titleService;
            IllustContentViewModel = new IllustContentViewModel(pixivClient, navigationService, objectCacheStorage);
            MangaContentViewModel = new MangaContentViewModel(pixivClient, navigationService, objectCacheStorage);
            SelectedTab = new ReactiveProperty<int>();
            SelectedTab.Subscribe(async w =>
            {
                if (w == 0)
                    await IllustContentViewModel.InitializeAsync();
                else if (w == 1)
                    await MangaContentViewModel.InitializeAsync();
            }).AddTo(this);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (e.Parameter != null)
                TransitionParameter.FromQueryString<TransitionParameter>(e.Parameter as string)?.ProcessTransitionHistory(_navigationService);

            _titleService.ViewTitle = "Shell_Home/Content".GetLocalized();
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}