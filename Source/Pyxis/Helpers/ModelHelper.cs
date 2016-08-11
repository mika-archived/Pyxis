using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;

using Windows.UI.Xaml.Data;

using Pyxis.Collections;

using Reactive.Bindings.Extensions;

namespace Pyxis.Helpers
{
    public static class ModelHelper
    {
        public static IDisposable ConnectTo<T, TU, TV>(IncrementalObservableCollection<T> collection,
                                                       TU connectTarget,
                                                       Expression<Func<TU, ObservableCollection<TV>>> expr,
                                                       Func<TV, T> converter)
            where T : class
            where TU : class, ISupportIncrementalLoading
        {
            collection.SupportIncrementalLoading = connectTarget;
            var obsCollection = expr.Compile().Invoke(connectTarget);
            var disposable = new CompositeDisposable
            {
                obsCollection.ObserveAddChanged().ObserveOnUIDispatcher().Subscribe(w => collection.Add(converter(w))),
                obsCollection.ObserveRemoveChanged().Subscribe(w => collection.Remove(converter(w))),
                obsCollection.ObserveResetChanged().Subscribe(w => collection.Clear())
            };
            return disposable;
        }
    }
}