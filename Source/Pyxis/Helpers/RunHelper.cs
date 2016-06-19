using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pyxis.Helpers
{
    internal static class RunHelper
    {
        public static void RunAsync(Func<Task> action)
        {
            Observable.Return(0).Subscribe(async w => await action.Invoke());
        }

        public static void RunAsync<T>(Func<T, Task> action, T param1)
        {
            Observable.Return(0).Subscribe(async w => await action.Invoke(param1));
        }

        public static void RunLater(Action action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).Subscribe(w => action.Invoke());
        }

        public static void RunLater<T>(Action<T> action, T param1, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).Subscribe(w => action.Invoke(param1));
        }

        public static void RunLaterAsync(Func<Task> action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).Subscribe(async w => await action.Invoke());
        }

        public static void RunLaterAsync<T>(Func<T, Task> action, T param1, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).Subscribe(async w => await action.Invoke(param1));
        }
    }
}