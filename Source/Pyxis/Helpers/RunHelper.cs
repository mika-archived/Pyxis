using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reactive.Bindings.Extensions;

// ReSharper disable InconsistentNaming

namespace Pyxis.Helpers
{
    internal static class RunHelper
    {
        /// <summary>
        ///     dueTime 秒経過した場合、 action を中断します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dueTime"></param>
        public static void Timeout(Action action, TimeSpan dueTime)
        {
            // ReSharper disable  MethodSupportsCancellation
            var tokenSource = new CancellationTokenSource();
            Observable.Return(0).Delay(dueTime).Subscribe(w =>
            {
                if (tokenSource == null)
                    return;
                tokenSource.Cancel();
                Debug.WriteLine("Timeout");
            });
            Task.Factory.StartNew(action.Invoke, tokenSource.Token)
                .ContinueWith(w =>
                {
                    tokenSource.Dispose();
                    tokenSource = null;
                })
                .Wait();
            // ReSharper restore  MethodSupportsCancellation
        }

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
        ///     バックグラウンドスレッド上で、 action を実行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param1"></param>
        public static void Run<T>(Action<T> action, T param1)
        {
            Observable.Timer(TimeSpan.Zero).Subscribe(w => action.Invoke(param1));
        }

        /// <summary>
        ///     バックグラウンドスレッド上で、非同期操作 action を実行します。
        /// </summary>
        /// <param name="action"></param>
        public static void RunAsync(Func<Task> action)
        {
            Observable.Timer(TimeSpan.Zero).Subscribe(async w => await action.Invoke());
        }

        /// <summary>
        ///     バックグラウンドスレッド上で、非同期操作 action を実行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param1"></param>
        public static void RunAsync<T>(Func<T, Task> action, T param1)
        {
            Observable.Timer(TimeSpan.Zero).Subscribe(async w => await action.Invoke(param1));
        }

        #endregion

        #region RunLater

        /// <summary>
        ///     action を　behind 後に実行します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="behind"></param>
        public static void RunLater(Action action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).Subscribe(w => action.Invoke());
        }

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
        ///     UI スレッド上で、 action を behind 後に実行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param1"></param>
        /// <param name="behind"></param>
        public static void RunLaterUI<T>(Action<T> action, T param1, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().Subscribe(w => action.Invoke(param1));
        }

        /// <summary>
        ///     UI スレッド上で、 非同期操作 action を behind 後に実行します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="behind"></param>
        public static void RunLaterUIAsync(Func<Task> action, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().Subscribe(async w => await action.Invoke());
        }

        /// <summary>
        ///     UI スレッド上で、 非同期操作 action を behind 後に実行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param1"></param>
        /// <param name="behind"></param>
        public static void RunLaterUIAsync<T>(Func<T, Task> action, T param1, TimeSpan behind)
        {
            Observable.Return(0).Delay(behind).ObserveOnUIDispatcher().Subscribe(async w => await action.Invoke(param1));
        }

        #endregion
    }
}