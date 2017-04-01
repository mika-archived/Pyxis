using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Windows.UI.Core;

using Pyxis.Models;

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
        public static void RunAsync(Func<Task> asyncAction)
        {
            Observable.Timer(TimeSpan.Zero).SelectMany(async w =>
            {
                await asyncAction.Invoke();
                return Unit.Default;
            }).Subscribe();
        }

        public static void RunOnUI(Action action)
        {
            Task.Run(() => PyxisConstants.UIDispatcher.RunAsync(CoreDispatcherPriority.Normal, action.Invoke)).Wait();
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
        public static void RunLaterUIAsync(Func<Task> action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().SelectMany(async w =>
            {
                await action.Invoke();
                return Unit.Default;
            }).Subscribe();
        }

        #endregion
    }
}