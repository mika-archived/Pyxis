using System.Diagnostics;

using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Dialogs
{
    public class SearchOptionDialogViewModel : DialogViewModel
    {
        public SearchOptionDialogViewModel()
        {
            Debug.WriteLine("Hello");
        }

        #region Overrides of DialogViewModel

        public override void OnInitialize(object parameter)
        {
            base.OnInitialize(parameter);
        }

        #endregion
    }
}