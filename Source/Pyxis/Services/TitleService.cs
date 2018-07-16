using Prism.Mvvm;

using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class TitleService : BindableBase, ITitleService
    {
        #region AppTitle

        private string _appTitle;

        public string AppTitle
        {
            get => _appTitle;
            set => SetProperty(ref _appTitle, value);
        }

        #endregion

        #region ViewTitle

        private string _viewTitle;

        public string ViewTitle
        {
            get => _viewTitle;
            set => SetProperty(ref _viewTitle, value);
        }

        #endregion
    }
}