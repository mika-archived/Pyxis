using System.Windows.Input;

using Prism.Commands;

namespace Pyxis.ViewModels
{
    public class ErrorDialogViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }

        #region PrimaryButtonCommand

        private ICommand _primaryButtonCommand;

        public ICommand PrimaryButtonCommand
            => _primaryButtonCommand ?? (_primaryButtonCommand = new DelegateCommand(PrimaryButtonClicked));

        private void PrimaryButtonClicked()
        {

        }

        #endregion
    }
}