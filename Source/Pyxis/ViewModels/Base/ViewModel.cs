using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using Windows.ApplicationModel.Resources;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Base
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        protected ResourceLoader Resources { get; }
        public CompositeDisposable CompositeDisposable { get; }

        protected ViewModel()
        {
            try
            {
                Resources = ResourceLoader.GetForCurrentView();
            }
            catch (Exception)
            {
                // ignored
            }
            CompositeDisposable = new CompositeDisposable();
        }

        #region Implementation of IDisposable

        public void Dispose() => CompositeDisposable.Dispose();

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
                                              bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (suspending)
                CompositeDisposable.Dispose();
        }

        #endregion
    }
}