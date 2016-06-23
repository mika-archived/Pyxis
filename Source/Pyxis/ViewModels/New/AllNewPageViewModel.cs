using System.Collections.Generic;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.New
{
    public class AllNewPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public AllNewPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private void Initialize(HomeParameter parameter)
        {
            SubSelectdIndex = (int) parameter.ContentType;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameters = ParameterBase.ToObject<HomeParameter>(e.Parameter?.ToString());
            Initialize(parameters);
        }

        #endregion

        #region SubSelectdIndex

        private int _subSelectedIndex;

        public int SubSelectdIndex
        {
            get { return _subSelectedIndex; }
            set { SetProperty(ref _subSelectedIndex, value); }
        }

        #endregion
    }
}