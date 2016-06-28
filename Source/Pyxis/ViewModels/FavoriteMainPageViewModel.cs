using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Collections;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels
{
    public class FavoriteMainPageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        private readonly IPixivClient _pixivClient;
        private FavoriteOptionParameter _favoriteOption;
        public INavigationService NavigationService { get; }

        public IncrementalObservableCollection<TappableThumbnailViewModel> FavoriteItems { get; }

        public FavoriteMainPageViewModel(IImageStoreService imageStoreService, IPixivClient pixivClient,
                                         INavigationService navigationService)
        {
            _imageStoreService = imageStoreService;
            _pixivClient = pixivClient;
            NavigationService = navigationService;
            FavoriteItems = new IncrementalObservableCollection<TappableThumbnailViewModel>();
        }

        private void Initialize(FavoriteOptionParameter parameter)
        {
            _favoriteOption = parameter;
            SelectedIndex = (int) parameter.Type;

            Sync();
        }

        private void GenerateQueries()
        {
            var param1 = _favoriteOption.Clone();
            param1.Type = SearchType.IllustsAndManga;

            var param2 = _favoriteOption.Clone();
            param2.Type = SearchType.Novels;

            ParameterQueries = new List<string>
            {
                (string) param1.ToJson(),
                (string) param2.ToJson()
            };
        }

        private void Sync()
        {
            GenerateQueries();
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<FavoriteOptionParameter>((string) e?.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion

        #region ParameterQueries

        private List<string> _parameterQueries;

        public List<string> ParameterQueries
        {
            get { return _parameterQueries; }
            set { SetProperty(ref _parameterQueries, value); }
        }

        #endregion
    }
}