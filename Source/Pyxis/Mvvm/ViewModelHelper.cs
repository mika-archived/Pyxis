using System;

using Pyxis.ViewModels.Base;

namespace Pyxis.Mvvm
{
    internal static class ViewModelHelper
    {
        public static T AddTo<T>(this T disposable, ViewModel viewModel) where T : IDisposable
        {
            viewModel.CompositeDisposable.Add(disposable);
            return disposable;
        }
    }
}