using Prism.Windows.Navigation;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Search
{
    public class UserSearchPageViewModel : ViewModel
    {
        public INavigationService NavigationService { get; }

        public UserSearchPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void OnQuerySubmitted()
        {

        }

        public void OnButtonTapped()
        {

        }

        #region QueryText

        private string _queryText;

        public string QueryText
        {
            get { return _queryText; }
            set { SetProperty(ref _queryText, value); }
        }

        #endregion
    }
}