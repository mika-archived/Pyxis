using System;
using System.Reactive.Disposables;

using Prism.Windows.Mvvm;

namespace Pyxis.ViewModels
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        public ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
        }

        public void Dispose()
        {
            CompositeDisposable.Dispose();
        }
    }
}