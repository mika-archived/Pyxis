using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Base
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        protected ViewModel()
        {
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