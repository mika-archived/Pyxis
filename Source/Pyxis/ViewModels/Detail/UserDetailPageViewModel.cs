using System.Collections.Generic;
using System.Diagnostics;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Detail
{
    public class UserDetailPageViewModel : ViewModel
    {
        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<UserDetailParameter>((string) e?.Parameter);
            Debug.WriteLine("");
        }

        #endregion
    }
}