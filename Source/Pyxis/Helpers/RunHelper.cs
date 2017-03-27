using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Reactive.Bindings.Extensions;

// ReSharper disable InconsistentNaming

namespace Pyxis.Helpers
{
    internal static class RunHelper
    {
        #region Run

        /// <summary>
        ///     バックグラウンドスレッド上で、 action を実行します。
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            Observable.Timer(TimeSpan.Zero).Subscribe(w => action.Invoke());
        }

        /// <summary>
        ///     バックグラウンドスレッド上で、非同期操作 action を実行します。
        /// </summary>
        /// <param name="asyncAction"></param>
        public static void RunAsync(Func<Task<Unit>> asyncAction)
        {
            Observable.Timer(TimeSpan.Zero).SelectMany(w => asyncAction.Invoke()).Subscribe();
        }

        #endregion

        #region RunLater

        /// <summary>
        ///     UI スレッド上で、 action を behind 後に実行します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="behind"></param>
        public static void RunLaterUI(Action action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().Subscribe(w => action.Invoke());
        }

        /// <summary>
        ///     UI スレッド上で、 非同期操作 action を behind 後に実行します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="behind"></param>
        public static void RunLaterUIAsync(Func<Task<Unit>> action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().SelectMany(w => action.Invoke()).Subscribe();
        }

        #endregion
    }
}