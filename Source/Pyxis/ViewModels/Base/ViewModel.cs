using System;
using System.Reactive.Disposables;

using Prism.Windows.Mvvm;

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
    }
}