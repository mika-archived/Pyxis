using System;
using System.Reactive.Disposables;

using Prism.Mvvm;

namespace Pyxis.ViewModels.Base
{
    internal class DialogViewModel : BindableBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        public object ResultValue { get; protected set; }

        protected DialogViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
        }

        #region Implementation of IDisposable

        public void Dispose() => CompositeDisposable.Dispose();

        #endregion

        public virtual void OnInitialize(object parameter)
        {

        }
    }
}