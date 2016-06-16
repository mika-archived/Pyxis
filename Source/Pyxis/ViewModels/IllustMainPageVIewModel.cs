using System.Collections.Generic;
using System.Diagnostics;

using Prism.Windows.Navigation;

namespace Pyxis.ViewModels
{
    public class IllustMainPageViewModel : ViewModel
    {
        public string Messaege => "Hello, world!";

        public IllustMainPageViewModel(INavigationService navigationService)
        {
            Debug.WriteLine("");
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Debug.WriteLine("");
        }

        #endregion
    }
}