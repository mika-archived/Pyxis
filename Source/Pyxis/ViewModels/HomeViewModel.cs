using System;
using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Extensions;
using Pyxis.Models;
using Pyxis.ViewModels.Home;

using Reactive.Bindings;

using Sagitta;

namespace Pyxis.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        public IllustContentViewModel IllustContentViewModel { get; }
        public ReactiveProperty<int> SelectedTab { get; }

        public HomeViewModel(PixivClient pixivClient, INavigationService navigationService)
        {
            _navigationService = navigationService;
            IllustContentViewModel = new IllustContentViewModel(pixivClient);
            SelectedTab = new ReactiveProperty<int>();
            SelectedTab.Subscribe(async w =>
            {
                if (w == 0)
                    await IllustContentViewModel.InitializeAsync();
            }).AddTo(this);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (e.Parameter != null)
                TransitionParameter.FromQueryString<TransitionParameter>(e.Parameter as string)?.ProcessTransitionHistory(_navigationService);

            base.OnNavigatedTo(e, viewModelState);
        }
    }
}