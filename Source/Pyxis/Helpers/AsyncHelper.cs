using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pyxis.Helpers
{
    internal static class AsyncHelper
    {
        public static void RunAsync(Func<Task> action)
        {
            Observable.Return(0).Subscribe(async w => await action.Invoke());
        }
    }
}